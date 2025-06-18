using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Modular.Charge.ChargeEngine.Configuration;
using Modular.Charge.ChargeEngine.Resilience;
using Modular.Charge.Domain.Entities;
using Modular.Charge.Domain.Repositories;
using Modular.Charge.Domain.Services;

namespace Modular.Charge.ChargeEngine.Services;

public class BatchProcessingService
{
    private readonly ILogger<BatchProcessingService> _logger;
    private readonly ISaleRepository _saleRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPaymentGateway _paymentGateway;
    private readonly ChargeEngineOptions _options;
    private readonly AsyncPolicy<bool> _resilientPolicy;

    public BatchProcessingService(
        ILogger<BatchProcessingService> logger,
        ISaleRepository saleRepository,
        ITransactionRepository transactionRepository,
        IPaymentGateway paymentGateway,
        IOptions<ChargeEngineOptions> options)
    {
        _logger = logger;
        _saleRepository = saleRepository;
        _transactionRepository = transactionRepository;
        _paymentGateway = paymentGateway;
        _options = options.Value;

        _resilientPolicy = ResiliencePolicies.CreateResilientPolicy<bool>(
            logger,
            _options.MaxRetries,
            _options.RetryDelay,
            _options.CircuitBreakerThreshold,
            _options.CircuitBreakerDuration,
            _options.Timeout);
    }

    public async Task ProcessBatchAsync(IEnumerable<Sale> sales, CancellationToken cancellationToken)
    {
        var salesList = sales.ToList();
        if (!salesList.Any())
        {
            _logger.LogDebug("No sales to process in batch");
            return;
        }

        _logger.LogInformation("Processing batch of {Count} sales", salesList.Count);

        var transactions = new List<Transaction>();
        var updates = new List<Sale>();

        foreach (var batch in salesList.Chunk(_options.BatchSize))
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            await ProcessBatchChunkAsync(batch, transactions, updates, cancellationToken);
        }

        // Persist all changes in a single transaction
        if (transactions.Any() || updates.Any())
        {
            await PersistBatchChangesAsync(transactions, updates);
        }
    }

    private async Task ProcessBatchChunkAsync(
        IEnumerable<Sale> batch,
        List<Transaction> transactions,
        List<Sale> updates,
        CancellationToken cancellationToken)
    {
        var tasks = batch.Select(sale => ProcessSaleWithResilienceAsync(sale, transactions, updates, cancellationToken));

        if (_options.EnableParallelProcessing)
        {
            await Task.WhenAll(tasks);
        }
        else
        {
            foreach (var task in tasks)
            {
                await task;
                if (cancellationToken.IsCancellationRequested)
                    break;
            }
        }
    }

    private async Task ProcessSaleWithResilienceAsync(
        Sale sale,
        List<Transaction> transactions,
        List<Sale> updates,
        CancellationToken cancellationToken)
    {
        try
        {
            var context = new Context($"ProcessSale_{sale.Id}");

            var result = await _resilientPolicy.ExecuteAsync(async (ctx) =>
            {
                return await ProcessSingleSaleAsync(sale, transactions, updates);
            }, context);

            if (result)
            {
                _logger.LogDebug("Successfully processed sale {SaleId}", sale.Id);
            }
            else
            {
                _logger.LogWarning("Failed to process sale {SaleId}", sale.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing sale {SaleId} with resilience policy", sale.Id);
        }
    }

    private async Task<bool> ProcessSingleSaleAsync(Sale sale, List<Transaction> transactions, List<Sale> updates)
    {
        try
        {
            // Create payment
            var paymentId = await _paymentGateway.CreatePaymentAsync(
                sale.Amount,
                sale.Currency,
                sale.Description);

            // Create transaction
            var transaction = new Transaction(
                sale.Id,
                sale.Amount,
                paymentId,
                sale.CompanyId);

            // Confirm payment
            var paymentConfirmed = await _paymentGateway.ConfirmPaymentAsync(paymentId);
            
            if (paymentConfirmed)
            {
                transaction.MarkAsProcessed();
                sale.MarkAsPaid();
            }
            else
            {
                transaction.MarkAsFailed();
                await _paymentGateway.CancelPaymentAsync(paymentId);
            }

            // Add to batch for later persistence
            lock (transactions)
            {
                transactions.Add(transaction);
            }

            lock (updates)
            {
                updates.Add(sale);
            }

            return paymentConfirmed;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing single sale {SaleId}", sale.Id);
            return false;
        }
    }

    private async Task PersistBatchChangesAsync(List<Transaction> transactions, List<Sale> updates)
    {
        try
        {
            _logger.LogInformation("Persisting {TransactionCount} transactions and {SaleCount} sales",
                transactions.Count, updates.Count);

            // Add all transactions
            foreach (var transaction in transactions)
            {
                await _transactionRepository.AddAsync(transaction);
            }

            // Update all sales
            foreach (var sale in updates)
            {
                await _saleRepository.UpdateAsync(sale);
            }

            // Save changes
            await _transactionRepository.SaveChangesAsync();
            await _saleRepository.SaveChangesAsync();

            _logger.LogInformation("Successfully persisted batch changes");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error persisting batch changes");
            throw;
        }
    }
} 
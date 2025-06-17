using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modular.Charge.Domain.Repositories;

namespace Modular.Charge.ChargeEngine.Services;

public class ChargeEngineService : BackgroundService
{
    private readonly ILogger<ChargeEngineService> _logger;
    private readonly ISaleRepository _saleRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPaymentGateway _paymentGateway;

    public ChargeEngineService(
        ILogger<ChargeEngineService> logger,
        ISaleRepository saleRepository,
        ITransactionRepository transactionRepository,
        IPaymentGateway paymentGateway)
    {
        _logger = logger;
        _saleRepository = saleRepository;
        _transactionRepository = transactionRepository;
        _paymentGateway = paymentGateway;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var pendingSales = await _saleRepository.GetPendingSalesAsync();
                foreach (var sale in pendingSales)
                {
                    try
                    {
                        var paymentId = await _paymentGateway.CreatePaymentAsync(
                            sale.Amount,
                            sale.Currency,
                            sale.Description);

                        var transaction = new Transaction(
                            sale.Id,
                            sale.Amount,
                            paymentId,
                            sale.CompanyId);

                        await _transactionRepository.AddAsync(transaction);

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

                        await _transactionRepository.UpdateAsync(transaction);
                        await _saleRepository.UpdateAsync(sale);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing sale {SaleId}", sale.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in charge engine");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
} 
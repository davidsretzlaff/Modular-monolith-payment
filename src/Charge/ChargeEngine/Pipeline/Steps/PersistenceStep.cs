using Microsoft.Extensions.Logging;
using Modular.Charge.Domain.Repositories;

namespace Modular.Charge.ChargeEngine.Pipeline.Steps;

public class PersistenceStep : IChargeProcessingStep
{
    private readonly ILogger<PersistenceStep> _logger;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ISaleRepository _saleRepository;

    public int Order => 4;

    public PersistenceStep(
        ILogger<PersistenceStep> logger,
        ITransactionRepository transactionRepository,
        ISaleRepository saleRepository)
    {
        _logger = logger;
        _transactionRepository = transactionRepository;
        _saleRepository = saleRepository;
    }

    public async Task<StepResult> ExecuteAsync(ChargeContext context)
    {
        try
        {
            if (context.Transaction == null || context.Sale == null)
            {
                return StepResult.Failure("Transaction or Sale not found in context");
            }

            await _transactionRepository.SaveChangesAsync();
            await _saleRepository.SaveChangesAsync();

            return StepResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error persisting changes for transaction {TransactionId}", context.Transaction?.Id);
            return StepResult.Retry($"Error persisting changes: {ex.Message}", TimeSpan.FromMinutes(5));
        }
    }
} 
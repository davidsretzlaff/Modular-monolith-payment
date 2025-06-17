using Microsoft.Extensions.Logging;
using Modular.Charge.Domain.Repositories;

namespace Modular.Charge.ChargeEngine.Pipeline.Steps;

public class ValidationStep : IChargeProcessingStep
{
    private readonly ILogger<ValidationStep> _logger;
    private readonly ISaleRepository _saleRepository;

    public int Order => 1;

    public ValidationStep(
        ILogger<ValidationStep> logger,
        ISaleRepository saleRepository)
    {
        _logger = logger;
        _saleRepository = saleRepository;
    }

    public async Task<StepResult> ExecuteAsync(ChargeContext context)
    {
        try
        {
            var sale = await _saleRepository.GetByIdAsync(context.Request.SaleId);
            if (sale == null)
            {
                return StepResult.Failure($"Sale {context.Request.SaleId} not found");
            }

            if (sale.Status != "PENDING")
            {
                return StepResult.Failure($"Sale {sale.Id} is not in PENDING status");
            }

            if (sale.Amount != context.Request.Amount)
            {
                return StepResult.Failure($"Sale amount {sale.Amount} does not match request amount {context.Request.Amount}");
            }

            context.Sale = sale;
            return StepResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating sale {SaleId}", context.Request.SaleId);
            return StepResult.Failure($"Error validating sale: {ex.Message}");
        }
    }
} 
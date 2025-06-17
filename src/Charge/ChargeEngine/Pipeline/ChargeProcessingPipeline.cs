using Microsoft.Extensions.Logging;
using Modular.Charge.ChargeEngine.Pipeline.Steps;

namespace Modular.Charge.ChargeEngine.Pipeline;

public class ChargeProcessingPipeline : IChargeProcessingPipeline
{
    private readonly ILogger<ChargeProcessingPipeline> _logger;
    private readonly IEnumerable<IChargeProcessingStep> _steps;

    public ChargeProcessingPipeline(
        ILogger<ChargeProcessingPipeline> logger,
        IEnumerable<IChargeProcessingStep> steps)
    {
        _logger = logger;
        _steps = steps.OrderBy(s => s.Order);
    }

    public async Task<StepResult> ExecuteAsync(ChargeContext context)
    {
        try
        {
            foreach (var step in _steps)
            {
                _logger.LogInformation(
                    "Executing step {StepName} for request {RequestId}",
                    step.GetType().Name,
                    context.Request.Id);

                var result = await step.ExecuteAsync(context);
                if (!result.Success)
                {
                    _logger.LogWarning(
                        "Step {StepName} failed for request {RequestId}: {ErrorMessage}",
                        step.GetType().Name,
                        context.Request.Id,
                        result.ErrorMessage);

                    return result;
                }
            }

            return StepResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Pipeline execution failed for request {RequestId}",
                context.Request.Id);

            return StepResult.Failure(
                $"Pipeline execution failed: {ex.Message}",
                TimeSpan.FromMinutes(5));
        }
    }
} 
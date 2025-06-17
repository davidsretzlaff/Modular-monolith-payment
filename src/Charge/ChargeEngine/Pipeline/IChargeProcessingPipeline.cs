namespace Modular.Charge.ChargeEngine.Pipeline;

public interface IChargeProcessingPipeline
{
    Task<StepResult> ExecuteAsync(ChargeContext context);
} 
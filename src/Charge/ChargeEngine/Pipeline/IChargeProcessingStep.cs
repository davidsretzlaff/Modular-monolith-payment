namespace Modular.Charge.ChargeEngine.Pipeline;

public interface IChargeProcessingStep
{
    int Order { get; }
    Task<StepResult> ExecuteAsync(ChargeContext context);
} 
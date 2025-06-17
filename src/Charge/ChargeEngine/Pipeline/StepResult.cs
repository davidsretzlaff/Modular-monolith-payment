namespace Modular.Charge.ChargeEngine.Pipeline;

public class StepResult
{
    public bool Success { get; }
    public string? ErrorMessage { get; }
    public bool ShouldRetry { get; }
    public TimeSpan? RetryAfter { get; }

    private StepResult(bool success, string? errorMessage = null, bool shouldRetry = false, TimeSpan? retryAfter = null)
    {
        Success = success;
        ErrorMessage = errorMessage;
        ShouldRetry = shouldRetry;
        RetryAfter = retryAfter;
    }

    public static StepResult Success() => new(true);

    public static StepResult Failure(string errorMessage, TimeSpan? retryAfter = null) =>
        new(false, errorMessage, retryAfter.HasValue, retryAfter);

    public static StepResult Retry(string errorMessage, TimeSpan retryAfter) =>
        new(false, errorMessage, true, retryAfter);
} 
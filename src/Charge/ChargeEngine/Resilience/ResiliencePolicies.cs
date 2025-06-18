using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Microsoft.Extensions.Logging;

namespace Modular.Charge.ChargeEngine.Resilience;

public static class ResiliencePolicies
{
    public static AsyncRetryPolicy<T> CreateRetryPolicy<T>(ILogger logger, int maxRetries, TimeSpan retryDelay)
    {
        return Policy<T>
            .Handle<Exception>()
            .WaitAndRetryAsync(
                maxRetries,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt - 1)) + retryDelay,
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    logger.LogWarning(
                        exception,
                        "Retry {RetryCount} after {Delay}ms for operation {OperationKey}",
                        retryCount,
                        timeSpan.TotalMilliseconds,
                        context.OperationKey);
                });
    }

    public static AsyncCircuitBreakerPolicy<T> CreateCircuitBreakerPolicy<T>(
        ILogger logger, 
        int threshold, 
        TimeSpan duration)
    {
        return Policy<T>
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: threshold,
                durationOfBreak: duration,
                onBreak: (exception, duration) =>
                {
                    logger.LogError(
                        exception,
                        "Circuit breaker opened for {Duration}ms",
                        duration.TotalMilliseconds);
                },
                onReset: () =>
                {
                    logger.LogInformation("Circuit breaker reset");
                },
                onHalfOpen: () =>
                {
                    logger.LogInformation("Circuit breaker half-open");
                });
    }

    public static AsyncPolicy<T> CreateTimeoutPolicy<T>(ILogger logger, TimeSpan timeout)
    {
        return Policy<T>
            .TimeoutAsync(timeout, onTimeoutAsync: (context, timeSpan, task) =>
            {
                logger.LogWarning(
                    "Operation {OperationKey} timed out after {Timeout}ms",
                    context.OperationKey,
                    timeSpan.TotalMilliseconds);
                return Task.CompletedTask;
            });
    }

    public static AsyncPolicy<T> CreateResilientPolicy<T>(
        ILogger logger,
        int maxRetries,
        TimeSpan retryDelay,
        int circuitBreakerThreshold,
        TimeSpan circuitBreakerDuration,
        TimeSpan timeout)
    {
        var retryPolicy = CreateRetryPolicy<T>(logger, maxRetries, retryDelay);
        var circuitBreakerPolicy = CreateCircuitBreakerPolicy<T>(logger, circuitBreakerThreshold, circuitBreakerDuration);
        var timeoutPolicy = CreateTimeoutPolicy<T>(logger, timeout);

        return Policy.WrapAsync(timeoutPolicy, circuitBreakerPolicy, retryPolicy);
    }
} 
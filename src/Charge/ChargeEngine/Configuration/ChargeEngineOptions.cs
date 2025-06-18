namespace Modular.Charge.ChargeEngine.Configuration;

public class ChargeEngineOptions
{
    public const string SectionName = "ChargeEngine";

    public int MaxConcurrency { get; set; } = 10;
    public int BatchSize { get; set; } = 50;
    public TimeSpan PollingInterval { get; set; } = TimeSpan.FromMinutes(1);
    public int MaxRetries { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(30);
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    public bool EnableCircuitBreaker { get; set; } = true;
    public int CircuitBreakerThreshold { get; set; } = 5;
    public TimeSpan CircuitBreakerDuration { get; set; } = TimeSpan.FromMinutes(1);
    public bool EnableBatching { get; set; } = true;
    public bool EnableParallelProcessing { get; set; } = true;
} 
using Shared.Core;

namespace Modular.Charge.ChargeEngine.Outbox;

public class ChargeOutbox
{
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public string Payload { get; private set; }
    public int RetryCount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public string? ErrorMessage { get; private set; }

    private ChargeOutbox() { }

    public static ChargeOutbox Create(string type, string payload)
    {
        return new ChargeOutbox
        {
            Id = Guid.NewGuid(),
            Type = type,
            Payload = payload,
            RetryCount = 0,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsProcessed()
    {
        ProcessedAt = DateTime.UtcNow;
    }

    public void IncrementRetryCount()
    {
        RetryCount++;
    }

    public void SetError(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
} 
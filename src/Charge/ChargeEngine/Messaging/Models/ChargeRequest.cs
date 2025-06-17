namespace Modular.Charge.ChargeEngine.Messaging.Models;

public class ChargeRequest
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "BRL";
    public string PaymentMethod { get; set; } = string.Empty;
    public Dictionary<string, string> PaymentMetadata { get; set; } = new();
    public int RetryCount { get; set; }
    public DateTime? ScheduledFor { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 
namespace Modular.Charge.ChargeEngine.Messaging.Models;

public class ChargeResult
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Guid SaleId { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string? TransactionId { get; set; }
    public string? PaymentId { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new();
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
} 
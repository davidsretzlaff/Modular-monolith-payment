using Shared.Core;

namespace Charge.Domain.Entities;

public class Transaction : Entity
{
    public Guid SaleId { get; private set; }
    public decimal Amount { get; private set; }
    public required string Currency { get; private set; }
    public required string Status { get; private set; }
    public required string PaymentId { get; private set; }
    public DateTime CreateDate { get; private set; }
    public DateTime? ProcessedDate { get; private set; }
    public Guid CompanyId { get; private set; }

    protected Transaction() { } // For EF

    public Transaction(
        Guid saleId,
        decimal amount,
        string paymentId,
        Guid companyId)
    {
        if (saleId == Guid.Empty)
            throw new ArgumentException("SaleId cannot be empty", nameof(saleId));

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero", nameof(amount));

        if (string.IsNullOrWhiteSpace(paymentId))
            throw new ArgumentException("PaymentId cannot be empty", nameof(paymentId));

        if (companyId == Guid.Empty)
            throw new ArgumentException("CompanyId cannot be empty", nameof(companyId));

        SaleId = saleId;
        Amount = amount;
        Currency = "BRL";
        PaymentId = paymentId;
        CompanyId = companyId;
        Status = "Pending";
        CreateDate = DateTime.UtcNow;
    }

    public void MarkAsProcessed()
    {
        if (Status == "Processed")
            throw new InvalidOperationException("Transaction is already processed");

        Status = "Processed";
        ProcessedDate = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public void MarkAsFailed()
    {
        if (Status == "Failed")
            throw new InvalidOperationException("Transaction is already failed");

        Status = "Failed";
        UpdateTimestamp();
    }
} 
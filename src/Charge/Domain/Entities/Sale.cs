using Modular.Shared.Domain;

namespace Modular.Charge.Domain.Entities;

public class Sale : Entity
{
    public Guid CustomerId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    public string Description { get; private set; }
    public bool IsPaid { get; private set; }
    public DateTime CreateDate { get; private set; }
    public DateTime? PaidDate { get; private set; }
    public Guid CompanyId { get; private set; }

    private readonly List<Transaction> _transactions = new();
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    protected Sale() { } // For EF

    public Sale(
        Guid customerId,
        decimal amount,
        string description,
        Guid companyId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty", nameof(customerId));

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero", nameof(amount));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        if (companyId == Guid.Empty)
            throw new ArgumentException("CompanyId cannot be empty", nameof(companyId));

        CustomerId = customerId;
        Amount = amount;
        Currency = "BRL";
        Description = description;
        CompanyId = companyId;
        IsPaid = false;
        CreateDate = DateTime.UtcNow;
    }

    public void MarkAsPaid()
    {
        if (IsPaid)
            throw new InvalidOperationException("Sale is already paid");

        IsPaid = true;
        PaidDate = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public void AddTransaction(Transaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        _transactions.Add(transaction);
        UpdateTimestamp();
    }
} 
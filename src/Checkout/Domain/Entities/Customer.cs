using Modular.Shared.Domain;

namespace Modular.Checkout.Domain.Entities;

public class Customer : Entity
{
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string Document { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CompanyId { get; private set; }
    public DateTime CreateDate { get; private set; }

    private readonly List<Subscription> _subscriptions = new();
    public IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();

    protected Customer() { } // For EF

    public Customer(
        string email,
        string name,
        string document,
        Guid companyId)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(document))
            throw new ArgumentException("Document cannot be empty", nameof(document));

        if (companyId == Guid.Empty)
            throw new ArgumentException("CompanyId cannot be empty", nameof(companyId));

        Email = email.ToLowerInvariant();
        Name = name;
        Document = document;
        CompanyId = companyId;
        IsActive = true;
        CreateDate = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        Name = name;
        Email = email.ToLowerInvariant();
        UpdateTimestamp();
    }

    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }
} 
using Shared.Core;
using System.Collections.Generic;

namespace Catalog.Domain.Entities;

public class Plan : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CompanyId { get; private set; }
    public int DurationInDays { get; private set; }

    private readonly List<PlanPricingOption> _pricingOptions = new();
    public IReadOnlyCollection<PlanPricingOption> PricingOptions => _pricingOptions.AsReadOnly();

    private Plan() : base() { } // For EF Core

    public Plan(string name, string description, Guid companyId, int durationInDays = 30) : base()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (companyId == Guid.Empty)
            throw new ArgumentException("CompanyId cannot be empty", nameof(companyId));
        
        if (durationInDays <= 0)
            throw new ArgumentException("Duration must be greater than zero", nameof(durationInDays));

        Name = name;
        Description = description;
        CompanyId = companyId;
        DurationInDays = durationInDays;
        IsActive = true;
    }

    public void AddPricingOption(decimal price, int billingCycleInMonths)
    {
        var pricingOption = new PlanPricingOption(Id, price, billingCycleInMonths);
        _pricingOptions.Add(pricingOption);
        UpdateTimestamp();
    }

    public void UpdateDetails(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        Name = name;
        Description = description;
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
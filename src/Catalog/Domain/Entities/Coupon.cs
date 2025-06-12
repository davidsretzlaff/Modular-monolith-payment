using Shared.Core;

namespace Catalog.Domain.Entities;

public class Coupon : Entity
{
    public string Code { get; private set; }
    public string Description { get; private set; }
    public decimal DiscountValue { get; private set; }
    public bool IsPercentage { get; private set; }
    public DateTime ValidFrom { get; private set; }
    public DateTime ValidUntil { get; private set; }
    public bool IsActive { get; private set; }
    public int UsageLimit { get; private set; }
    public int UsedCount { get; private set; }
    public Guid? PlanId { get; private set; } // Coupon can be restricted to a specific plan

    private Coupon() : base() { } // For EF Core

    public Coupon(string code, string description, decimal discountValue, bool isPercentage,
                 DateTime validFrom, DateTime validUntil, int usageLimit = 0, Guid? planId = null) : base()
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty", nameof(code));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (discountValue <= 0)
            throw new ArgumentException("Discount value must be greater than zero", nameof(discountValue));
        
        if (isPercentage && discountValue > 100)
            throw new ArgumentException("Percentage discount cannot be greater than 100", nameof(discountValue));
        
        if (validFrom >= validUntil)
            throw new ArgumentException("Valid from must be before valid until", nameof(validFrom));

        Code = code.ToUpperInvariant();
        Description = description;
        DiscountValue = discountValue;
        IsPercentage = isPercentage;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
        UsageLimit = usageLimit;
        UsedCount = 0;
        IsActive = true;
        PlanId = planId;
    }

    public bool IsValid(DateTime? date = null)
    {
        var checkDate = date ?? DateTime.UtcNow;
        return IsActive && 
               checkDate >= ValidFrom && 
               checkDate <= ValidUntil &&
               (UsageLimit == 0 || UsedCount < UsageLimit);
    }

    public bool IsValidForPlan(Guid planId, DateTime? date = null)
    {
        return IsValid(date) && (PlanId == null || PlanId == planId);
    }

    public decimal CalculateDiscount(decimal originalValue)
    {
        if (!IsValid())
            return 0;

        if (IsPercentage)
            return originalValue * (DiscountValue / 100);
        
        return Math.Min(DiscountValue, originalValue);
    }

    public void Use()
    {
        if (!IsValid())
            throw new InvalidOperationException("Cannot use invalid coupon");
        
        UsedCount++;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }
} 
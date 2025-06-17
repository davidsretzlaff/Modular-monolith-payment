using Modular.Shared.Domain;

namespace Modular.Catalog.Domain.Entities;

public class PlanPricingOption : Entity
{
    public Guid PlanId { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = "BRL";
    public int BillingCycleInMonths { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreateDate { get; private set; }

    // Propriedade de navegação
    public virtual Plan Plan { get; private set; } = null!;

    protected PlanPricingOption() { } // Para o EF

    public PlanPricingOption(
        Guid planId,
        decimal price,
        int billingCycleInMonths)
    {
        PlanId = planId;
        Price = price;
        BillingCycleInMonths = billingCycleInMonths;
        IsActive = true;
        CreateDate = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void UpdatePrice(decimal newPrice)
    {
        Price = newPrice;
    }
} 
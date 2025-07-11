using Shared.Core;

namespace Checkout.Domain.Entities;

public class Subscription : Entity
{
    public Guid CustomerId { get; private set; }
    public Guid PlanId { get; private set; }
    public Guid? CouponId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public DateTime CreateDate { get; private set; }
    public Guid CompanyId { get; private set; }

    // Propriedades de navegação
    public virtual Customer Customer { get; private set; } = null!;

    protected Subscription() { } // For EF

    public Subscription(
        Guid customerId,
        Guid planId,
        Guid? couponId,
        Guid companyId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty", nameof(customerId));

        if (planId == Guid.Empty)
            throw new ArgumentException("PlanId cannot be empty", nameof(planId));

        if (companyId == Guid.Empty)
            throw new ArgumentException("CompanyId cannot be empty", nameof(companyId));

        CustomerId = customerId;
        PlanId = planId;
        CouponId = couponId;
        CompanyId = companyId;
        IsActive = true;
        StartDate = DateTime.UtcNow;
        CreateDate = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (!IsActive)
            throw new InvalidOperationException("Subscription is already inactive");

        IsActive = false;
        EndDate = DateTime.UtcNow;
        UpdateTimestamp();
    }

    public void Reactivate()
    {
        if (IsActive)
            throw new InvalidOperationException("Subscription is already active");

        IsActive = true;
        EndDate = null;
        UpdateTimestamp();
    }
} 
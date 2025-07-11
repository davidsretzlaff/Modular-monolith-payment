namespace Modular.Checkout.Application.Dtos;

public class SubscriptionWithPlanDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid PlanId { get; set; }
    public Guid? CouponId { get; set; }
    public bool IsActive { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid CompanyId { get; set; }
    public PlanDto Plan { get; set; } = null!;
    public CouponDto? Coupon { get; set; }
} 
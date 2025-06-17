namespace Modular.Checkout.Application.Dtos;

public class CreateSubscriptionDto
{
    public Guid CustomerId { get; set; }
    public Guid PlanId { get; set; }
    public Guid? CouponId { get; set; }
} 
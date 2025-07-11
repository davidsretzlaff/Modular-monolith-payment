using Shared.Core.Cqrs;

namespace Checkout.Application.Commands.Subscriptions;

public class CreateSubscriptionCommand : ICommand<Guid>
{
    public Guid CustomerId { get; set; }
    public Guid PlanId { get; set; }
    public Guid? CouponId { get; set; }
    public Guid CompanyId { get; set; }
} 
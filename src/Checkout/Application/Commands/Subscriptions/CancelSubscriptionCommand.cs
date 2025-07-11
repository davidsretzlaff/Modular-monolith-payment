using Shared.Core.Cqrs;

namespace Checkout.Application.Commands.Subscriptions;

public class CancelSubscriptionCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
}

public class ReactivateSubscriptionCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
} 
using Checkout.Domain.Repositories;
using Shared.Core.Cqrs;

namespace Checkout.Application.Commands.Subscriptions;

public class CancelSubscriptionCommandHandler : ICommandHandler<CancelSubscriptionCommand>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public CancelSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task HandleAsync(CancelSubscriptionCommand command)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (subscription == null)
            throw new InvalidOperationException("Subscription not found");

        subscription.Cancel();
        
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _subscriptionRepository.SaveChangesAsync();
    }
}

public class ReactivateSubscriptionCommandHandler : ICommandHandler<ReactivateSubscriptionCommand>
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public ReactivateSubscriptionCommandHandler(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task HandleAsync(ReactivateSubscriptionCommand command)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (subscription == null)
            throw new InvalidOperationException("Subscription not found");

        subscription.Reactivate();
        
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _subscriptionRepository.SaveChangesAsync();
    }
} 
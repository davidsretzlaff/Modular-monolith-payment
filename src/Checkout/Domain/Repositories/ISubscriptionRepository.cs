using Modular.Checkout.Domain.Entities;

namespace Modular.Checkout.Domain.Repositories;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetByIdAsync(Guid id, Guid companyId);
    Task<Subscription?> GetActiveByCustomerIdAsync(Guid customerId, Guid companyId);
    Task AddAsync(Subscription subscription);
    Task UpdateAsync(Subscription subscription);
} 
using Microsoft.EntityFrameworkCore;
using Modular.Checkout.Domain.Entities;
using Modular.Checkout.Domain.Repositories;
using Modular.Shared.Infrastructure.Data;

namespace Modular.Checkout.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly ApplicationDbContext _context;

    public SubscriptionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Subscription?> GetByIdAsync(Guid id, Guid companyId)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId);
    }

    public async Task<Subscription?> GetActiveByCustomerIdAsync(Guid customerId, Guid companyId)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.CompanyId == companyId && x.IsActive);
    }

    public async Task AddAsync(Subscription subscription)
    {
        await _context.Subscriptions.AddAsync(subscription);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subscription subscription)
    {
        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync();
    }
} 
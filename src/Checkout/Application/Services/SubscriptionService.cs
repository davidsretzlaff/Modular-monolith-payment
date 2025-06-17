using Modular.Catalog.Domain.Repositories;
using Modular.Checkout.Application.Dtos;
using Modular.Checkout.Application.Services;
using Modular.Checkout.Domain.Entities;
using Modular.Checkout.Domain.Repositories;

namespace Modular.Checkout.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPlanRepository _planRepository;
    private readonly ICouponRepository _couponRepository;

    public SubscriptionService(
        ISubscriptionRepository subscriptionRepository,
        ICustomerRepository customerRepository,
        IPlanRepository planRepository,
        ICouponRepository couponRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _customerRepository = customerRepository;
        _planRepository = planRepository;
        _couponRepository = couponRepository;
    }

    public async Task<Guid> CreateAsync(CreateSubscriptionDto dto, Guid companyId)
    {
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId, companyId);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        if (!customer.IsActive)
            throw new InvalidOperationException("Customer is inactive");

        var plan = await _planRepository.GetByIdAsync(dto.PlanId, companyId);
        if (plan == null)
            throw new InvalidOperationException("Plan not found");

        if (!plan.IsActive)
            throw new InvalidOperationException("Plan is inactive");

        if (dto.CouponId.HasValue)
        {
            var coupon = await _couponRepository.GetByIdAsync(dto.CouponId.Value, companyId);
            if (coupon == null)
                throw new InvalidOperationException("Coupon not found");

            if (!coupon.IsActive)
                throw new InvalidOperationException("Coupon is inactive");
        }

        var activeSubscription = await _subscriptionRepository.GetActiveByCustomerIdAsync(dto.CustomerId, companyId);
        if (activeSubscription != null)
            throw new InvalidOperationException("Customer already has an active subscription");

        var subscription = new Subscription(dto.CustomerId, dto.PlanId, dto.CouponId, companyId);
        await _subscriptionRepository.AddAsync(subscription);

        return subscription.Id;
    }

    public async Task CancelAsync(Guid id, Guid companyId)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, companyId);
        if (subscription == null)
            throw new InvalidOperationException("Subscription not found");

        subscription.Cancel();
        await _subscriptionRepository.UpdateAsync(subscription);
    }

    public async Task ReactivateAsync(Guid id, Guid companyId)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, companyId);
        if (subscription == null)
            throw new InvalidOperationException("Subscription not found");

        subscription.Reactivate();
        await _subscriptionRepository.UpdateAsync(subscription);
    }
} 
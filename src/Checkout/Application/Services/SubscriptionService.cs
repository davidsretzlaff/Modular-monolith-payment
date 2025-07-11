using Shared.Contracts;
using Modular.Checkout.Application.Dtos;
using Modular.Checkout.Application.Services;
using Modular.Checkout.Domain.Entities;
using Modular.Checkout.Domain.Repositories;

namespace Modular.Checkout.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICatalogApiProvider _catalogApiProvider;
    private readonly IPaymentGateway _paymentGateway;

    public SubscriptionService(
        ISubscriptionRepository subscriptionRepository,
        ICustomerRepository customerRepository,
        ICatalogApiProvider catalogApiProvider,
        IPaymentGateway paymentGateway)
    {
        _subscriptionRepository = subscriptionRepository;
        _customerRepository = customerRepository;
        _catalogApiProvider = catalogApiProvider;
        _paymentGateway = paymentGateway;
    }

    public async Task<Guid> CreateAsync(CreateSubscriptionDto dto, Guid companyId)
    {
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId, companyId);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        if (!customer.IsActive)
            throw new InvalidOperationException("Customer is inactive");

        // Validate plan exists and is active via Catalog API
        var planExists = await _catalogApiProvider.PlanExistsAsync(dto.PlanId, companyId);
        if (!planExists)
            throw new InvalidOperationException("Plan not found");

        var planIsActive = await _catalogApiProvider.IsPlanActiveAsync(dto.PlanId, companyId);
        if (!planIsActive)
            throw new InvalidOperationException("Plan is inactive");

        // Validate coupon if provided
        if (dto.CouponId.HasValue)
        {
            var couponExists = await _catalogApiProvider.CouponExistsAsync(dto.CouponId.Value, companyId);
            if (!couponExists)
                throw new InvalidOperationException("Coupon not found");

            var couponIsActive = await _catalogApiProvider.IsCouponActiveAsync(dto.CouponId.Value, companyId);
            if (!couponIsActive)
                throw new InvalidOperationException("Coupon is inactive");

            // Validate coupon is valid for the plan
            var couponValidForPlan = await _catalogApiProvider.IsCouponValidForPlanAsync(dto.CouponId.Value, dto.PlanId);
            if (!couponValidForPlan)
                throw new InvalidOperationException("Coupon is not valid for this plan");
        }

        var activeSubscription = await _subscriptionRepository.GetActiveByCustomerIdAsync(dto.CustomerId, companyId);
        if (activeSubscription != null)
            throw new InvalidOperationException("Customer already has an active subscription");

        // Get plan info for payment
        var planInfo = await _catalogApiProvider.GetPlanBasicInfoAsync(dto.PlanId, companyId);
        if (planInfo == null)
            throw new InvalidOperationException("Plan information not available");

        var subscription = new Subscription(dto.CustomerId, dto.PlanId, dto.CouponId, companyId);
        await _subscriptionRepository.AddAsync(subscription);

        // Calculate final price (with discount if coupon is applied)
        decimal finalPrice = planInfo.Price;
        if (dto.CouponId.HasValue)
        {
            var discountAmount = await _catalogApiProvider.CalculateDiscountAsync(dto.CouponId.Value, planInfo.Price);
            finalPrice = planInfo.Price - discountAmount;
        }

        var paymentId = await _paymentGateway.CreatePaymentAsync(
            finalPrice,
            "BRL",
            $"Subscription to plan {planInfo.Name}");

        var paymentConfirmed = await _paymentGateway.ConfirmPaymentAsync(paymentId);
        if (!paymentConfirmed)
        {
            await _paymentGateway.CancelPaymentAsync(paymentId);
            subscription.Cancel();
            await _subscriptionRepository.UpdateAsync(subscription);
            throw new InvalidOperationException("Payment failed");
        }

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
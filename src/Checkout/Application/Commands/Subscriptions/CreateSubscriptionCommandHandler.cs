using Checkout.Domain.Entities;
using Checkout.Domain.Repositories;
using Shared.Core.Cqrs;
using Shared.Contracts;

namespace Checkout.Application.Commands.Subscriptions;

public class CreateSubscriptionCommandHandler : ICommandHandler<CreateSubscriptionCommand, Guid>
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICatalogApiProvider _catalogApiProvider;
    private readonly IPaymentGateway _paymentGateway;

    public CreateSubscriptionCommandHandler(
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

    public async Task<Guid> HandleAsync(CreateSubscriptionCommand command)
    {
        // Validate customer exists and is active
        var customer = await _customerRepository.GetByIdAsync(command.CustomerId, command.CompanyId);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        if (!customer.IsActive)
            throw new InvalidOperationException("Customer is inactive");

        // Validate plan exists and is active via Catalog API
        var planExists = await _catalogApiProvider.PlanExistsAsync(command.PlanId, command.CompanyId);
        if (!planExists)
            throw new InvalidOperationException("Plan not found");

        var planIsActive = await _catalogApiProvider.IsPlanActiveAsync(command.PlanId, command.CompanyId);
        if (!planIsActive)
            throw new InvalidOperationException("Plan is inactive");

        // Validate coupon if provided
        if (command.CouponId.HasValue)
        {
            var couponExists = await _catalogApiProvider.CouponExistsAsync(command.CouponId.Value, command.CompanyId);
            if (!couponExists)
                throw new InvalidOperationException("Coupon not found");

            var couponIsActive = await _catalogApiProvider.IsCouponActiveAsync(command.CouponId.Value, command.CompanyId);
            if (!couponIsActive)
                throw new InvalidOperationException("Coupon is inactive");

            // Validate coupon is valid for the plan
            var couponValidForPlan = await _catalogApiProvider.IsCouponValidForPlanAsync(command.CouponId.Value, command.PlanId);
            if (!couponValidForPlan)
                throw new InvalidOperationException("Coupon is not valid for this plan");
        }

        // Check if customer already has an active subscription
        var activeSubscription = await _subscriptionRepository.GetActiveByCustomerIdAsync(command.CustomerId, command.CompanyId);
        if (activeSubscription != null)
            throw new InvalidOperationException("Customer already has an active subscription");

        // Get plan info for payment
        var planInfo = await _catalogApiProvider.GetPlanBasicInfoAsync(command.PlanId, command.CompanyId);
        if (planInfo == null)
            throw new InvalidOperationException("Plan information not available");

        var subscription = new Subscription(command.CustomerId, command.PlanId, command.CouponId, command.CompanyId);
        await _subscriptionRepository.AddAsync(subscription);

        // Calculate final price (with discount if coupon is applied)
        decimal finalPrice = planInfo.Price;
        if (command.CouponId.HasValue)
        {
            var discountAmount = await _catalogApiProvider.CalculateDiscountAsync(command.CouponId.Value, planInfo.Price);
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
            // No need for UpdateAsync with tracking enabled - EF will detect changes
            await _subscriptionRepository.SaveChangesAsync();
            throw new InvalidOperationException("Payment failed");
        }

        return subscription.Id;
    }
} 
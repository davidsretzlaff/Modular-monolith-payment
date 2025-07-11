namespace Checkout.Application.Integration.Provider;

public interface ICheckoutApiProvider
{
    Task<bool> CustomerExistsAsync(Guid customerId);
    Task<bool> SubscriptionExistsAsync(Guid subscriptionId);
    Task<bool> IsCustomerActiveAsync(Guid customerId);
    Task<bool> IsSubscriptionActiveAsync(Guid subscriptionId);
    Task<CustomerBasicInfo?> GetCustomerBasicInfoAsync(Guid customerId);
    Task<SubscriptionBasicInfo?> GetSubscriptionBasicInfoAsync(Guid subscriptionId);
}

public class CustomerBasicInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid CompanyId { get; set; }
}

public class SubscriptionBasicInfo
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid PlanId { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
} 
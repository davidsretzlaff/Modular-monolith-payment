using Checkout.Application.Dtos;
using Shared.Core.Cqrs;

namespace Checkout.Application.Queries.Subscriptions;

public class GetSubscriptionByIdQuery : IQuery<SubscriptionDto?>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }

    public GetSubscriptionByIdQuery(Guid id, Guid companyId)
    {
        Id = id;
        CompanyId = companyId;
    }
}

public class GetSubscriptionsByCustomerIdQuery : IQuery<IEnumerable<SubscriptionDto>>
{
    public Guid CustomerId { get; set; }
    public Guid CompanyId { get; set; }

    public GetSubscriptionsByCustomerIdQuery(Guid customerId, Guid companyId)
    {
        CustomerId = customerId;
        CompanyId = companyId;
    }
}

public class GetActiveSubscriptionsQuery : IQuery<IEnumerable<SubscriptionDto>>
{
    public Guid CompanyId { get; set; }

    public GetActiveSubscriptionsQuery(Guid companyId)
    {
        CompanyId = companyId;
    }
}

public class GetSubscriptionWithPlanQuery : IQuery<SubscriptionWithPlanDto?>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }

    public GetSubscriptionWithPlanQuery(Guid id, Guid companyId)
    {
        Id = id;
        CompanyId = companyId;
    }
} 
using Checkout.Application.Dtos;
using Dapper;
using Shared.Core.Cqrs;
using System.Data;

namespace Checkout.Application.Queries.Subscriptions;

public class GetSubscriptionByIdQueryHandler : IQueryHandler<GetSubscriptionByIdQuery, SubscriptionDto?>
{
    private readonly IDbConnection _dbConnection;

    public GetSubscriptionByIdQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<SubscriptionDto?> HandleAsync(GetSubscriptionByIdQuery query)
    {
        const string sql = @"
            SELECT s.Id, s.CustomerId, s.PlanId, s.CouponId, s.CompanyId, 
                   s.IsActive, s.Status, s.CreatedAt, s.UpdatedAt
            FROM Subscriptions s
            WHERE s.Id = @Id AND s.CompanyId = @CompanyId";

        return await _dbConnection.QuerySingleOrDefaultAsync<SubscriptionDto>(sql, 
            new { Id = query.Id, CompanyId = query.CompanyId });
    }
}

public class GetSubscriptionsByCustomerIdQueryHandler : IQueryHandler<GetSubscriptionsByCustomerIdQuery, IEnumerable<SubscriptionDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetSubscriptionsByCustomerIdQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<SubscriptionDto>> HandleAsync(GetSubscriptionsByCustomerIdQuery query)
    {
        const string sql = @"
            SELECT s.Id, s.CustomerId, s.PlanId, s.CouponId, s.CompanyId, 
                   s.IsActive, s.Status, s.CreatedAt, s.UpdatedAt
            FROM Subscriptions s
            WHERE s.CustomerId = @CustomerId AND s.CompanyId = @CompanyId
            ORDER BY s.CreatedAt DESC";

        return await _dbConnection.QueryAsync<SubscriptionDto>(sql, 
            new { CustomerId = query.CustomerId, CompanyId = query.CompanyId });
    }
}

public class GetActiveSubscriptionsQueryHandler : IQueryHandler<GetActiveSubscriptionsQuery, IEnumerable<SubscriptionDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetActiveSubscriptionsQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<SubscriptionDto>> HandleAsync(GetActiveSubscriptionsQuery query)
    {
        const string sql = @"
            SELECT s.Id, s.CustomerId, s.PlanId, s.CouponId, s.CompanyId, 
                   s.IsActive, s.Status, s.CreatedAt, s.UpdatedAt
            FROM Subscriptions s
            WHERE s.CompanyId = @CompanyId AND s.IsActive = 1
            ORDER BY s.CreatedAt DESC";

        return await _dbConnection.QueryAsync<SubscriptionDto>(sql, 
            new { CompanyId = query.CompanyId });
    }
}

public class GetSubscriptionWithPlanQueryHandler : IQueryHandler<GetSubscriptionWithPlanQuery, SubscriptionWithPlanDto?>
{
    private readonly IDbConnection _dbConnection;

    public GetSubscriptionWithPlanQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<SubscriptionWithPlanDto?> HandleAsync(GetSubscriptionWithPlanQuery query)
    {
        const string sql = @"
            SELECT s.Id, s.CustomerId, s.PlanId, s.CouponId, s.CompanyId, 
                   s.IsActive, s.Status, s.CreatedAt, s.UpdatedAt,
                   c.Id, c.Name, c.Email, c.IsActive, c.CompanyId, c.CreatedAt, c.UpdatedAt,
                   p.Id, p.Name, p.Description, p.Price, p.IsActive, 
                   p.CompanyId, p.DurationInDays, p.CreatedAt, p.UpdatedAt
            FROM Subscriptions s
            LEFT JOIN Customers c ON s.CustomerId = c.Id
            LEFT JOIN Plans p ON s.PlanId = p.Id
            WHERE s.Id = @Id AND s.CompanyId = @CompanyId";

        var subscriptionDict = new Dictionary<Guid, SubscriptionWithPlanDto>();

        await _dbConnection.QueryAsync<SubscriptionWithPlanDto, CustomerDto, PlanDto, SubscriptionWithPlanDto>(sql,
            (subscription, customer, plan) =>
            {
                if (!subscriptionDict.TryGetValue(subscription.Id, out var subscriptionEntry))
                {
                    subscriptionEntry = subscription;
                    subscriptionEntry.Customer = customer;
                    subscriptionEntry.Plan = plan;
                    subscriptionDict.Add(subscription.Id, subscriptionEntry);
                }
                return subscriptionEntry;
            },
            new { Id = query.Id, CompanyId = query.CompanyId },
            splitOn: "Id,Id");

        return subscriptionDict.Values.FirstOrDefault();
    }
} 
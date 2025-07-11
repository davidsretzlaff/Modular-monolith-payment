using Dapper;
using Microsoft.Data.SqlClient;
using Modular.Checkout.Application.Dtos;
using System.Data;

namespace Modular.Checkout.Application.Queries;

public class GetSubscriptionWithPlanQuery
{
    public Guid SubscriptionId { get; set; }
    public Guid CompanyId { get; set; }
}

public class GetSubscriptionWithPlanQueryHandler
{
    private readonly string _connectionString;
    public GetSubscriptionWithPlanQueryHandler(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<SubscriptionWithPlanDto?> Handle(GetSubscriptionWithPlanQuery query)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = @"
            SELECT s.Id, s.CustomerId, s.PlanId, s.CouponId, s.IsActive, s.StartDate, s.EndDate, s.CreateDate, s.CompanyId,
                   p.Id as Plan_Id, p.Name as Plan_Name, p.Description as Plan_Description, p.Price as Plan_Price, p.IsActive as Plan_IsActive, p.CompanyId as Plan_CompanyId, p.DurationInDays as Plan_DurationInDays, p.CreatedAt as Plan_CreatedAt, p.UpdatedAt as Plan_UpdatedAt,
                   c.Id as Coupon_Id, c.Code as Coupon_Code, c.Description as Coupon_Description, c.DiscountValue as Coupon_DiscountValue, c.IsPercentage as Coupon_IsPercentage, c.ValidFrom as Coupon_ValidFrom, c.ValidUntil as Coupon_ValidUntil, c.IsActive as Coupon_IsActive, c.UsageLimit as Coupon_UsageLimit, c.UsedCount as Coupon_UsedCount, c.PlanId as Coupon_PlanId, c.CreatedAt as Coupon_CreatedAt, c.UpdatedAt as Coupon_UpdatedAt
            FROM Subscriptions s
            INNER JOIN Plans p ON s.PlanId = p.Id
            LEFT JOIN Coupons c ON s.CouponId = c.Id
            WHERE s.Id = @SubscriptionId AND s.CompanyId = @CompanyId
        ";
        var result = await connection.QueryAsync<SubscriptionWithPlanDto, PlanDto, CouponDto, SubscriptionWithPlanDto>(
            sql,
            (subscription, plan, coupon) =>
            {
                subscription.Plan = plan;
                subscription.Coupon = coupon;
                return subscription;
            },
            new { query.SubscriptionId, query.CompanyId },
            splitOn: "Plan_Id,Coupon_Id"
        );
        return result.FirstOrDefault();
    }
} 
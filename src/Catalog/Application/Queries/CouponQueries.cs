using Catalog.Application.Dtos;
using Dapper;
using System.Data;

namespace Catalog.Application.Queries;

public interface ICouponQueries
{
    Task<CouponDto?> GetByIdAsync(Guid id);
    Task<CouponDto?> GetByCodeAsync(string code);
    Task<IEnumerable<CouponDto>> GetActiveAsync();
    Task<IEnumerable<CouponDto>> GetAllAsync();
    Task<IEnumerable<CouponDto>> GetByPlanIdAsync(Guid planId);
}

public class CouponQueries : ICouponQueries
{
    private readonly IDbConnection _dbConnection;

    public CouponQueries(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<CouponDto?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT c.Id, c.Code, c.Description, c.DiscountValue, c.IsPercentage, 
                   c.ValidFrom, c.ValidUntil, c.IsActive, c.UsageLimit, c.UsedCount, 
                   c.PlanId, c.CreatedAt, c.UpdatedAt,
                   p.Id, p.Name, p.Description, p.Price, p.IsActive, p.CompanyId, p.CreatedAt, p.UpdatedAt
            FROM Coupons c
            LEFT JOIN Plans p ON c.PlanId = p.Id
            WHERE c.Id = @Id";

        var couponDict = new Dictionary<Guid, CouponDto>();

        await _dbConnection.QueryAsync<CouponDto, PlanDto, CouponDto>(sql,
            (coupon, plan) =>
            {
                if (!couponDict.TryGetValue(coupon.Id, out var couponEntry))
                {
                    couponEntry = coupon;
                    couponEntry.Plan = plan;
                    couponDict.Add(coupon.Id, couponEntry);
                }
                return couponEntry;
            },
            new { Id = id },
            splitOn: "Id");

        return couponDict.Values.FirstOrDefault();
    }

    public async Task<CouponDto?> GetByCodeAsync(string code)
    {
        const string sql = @"
            SELECT c.Id, c.Code, c.Description, c.DiscountValue, c.IsPercentage, 
                   c.ValidFrom, c.ValidUntil, c.IsActive, c.UsageLimit, c.UsedCount, 
                   c.PlanId, c.CreatedAt, c.UpdatedAt,
                   p.Id, p.Name, p.Description, p.Price, p.IsActive, p.CompanyId, p.CreatedAt, p.UpdatedAt
            FROM Coupons c
            LEFT JOIN Plans p ON c.PlanId = p.Id
            WHERE c.Code = @Code";

        var couponDict = new Dictionary<Guid, CouponDto>();

        await _dbConnection.QueryAsync<CouponDto, PlanDto, CouponDto>(sql,
            (coupon, plan) =>
            {
                if (!couponDict.TryGetValue(coupon.Id, out var couponEntry))
                {
                    couponEntry = coupon;
                    couponEntry.Plan = plan;
                    couponDict.Add(coupon.Id, couponEntry);
                }
                return couponEntry;
            },
            new { Code = code.ToUpperInvariant() },
            splitOn: "Id");

        return couponDict.Values.FirstOrDefault();
    }

    public async Task<IEnumerable<CouponDto>> GetActiveAsync()
    {
        const string sql = @"
            SELECT c.Id, c.Code, c.Description, c.DiscountValue, c.IsPercentage, 
                   c.ValidFrom, c.ValidUntil, c.IsActive, c.UsageLimit, c.UsedCount, 
                   c.PlanId, c.CreatedAt, c.UpdatedAt,
                   p.Id, p.Name, p.Description, p.Price, p.IsActive, p.CompanyId, p.CreatedAt, p.UpdatedAt
            FROM Coupons c
            LEFT JOIN Plans p ON c.PlanId = p.Id
            WHERE c.IsActive = 1 
              AND c.ValidFrom <= GETUTCDATE() 
              AND c.ValidUntil >= GETUTCDATE()
              AND (c.UsageLimit = 0 OR c.UsedCount < c.UsageLimit)
            ORDER BY c.CreatedAt DESC";

        var couponDict = new Dictionary<Guid, CouponDto>();

        await _dbConnection.QueryAsync<CouponDto, PlanDto, CouponDto>(sql,
            (coupon, plan) =>
            {
                if (!couponDict.TryGetValue(coupon.Id, out var couponEntry))
                {
                    couponEntry = coupon;
                    couponEntry.Plan = plan;
                    couponDict.Add(coupon.Id, couponEntry);
                }
                return couponEntry;
            },
            splitOn: "Id");

        return couponDict.Values;
    }

    public async Task<IEnumerable<CouponDto>> GetAllAsync()
    {
        const string sql = @"
            SELECT c.Id, c.Code, c.Description, c.DiscountValue, c.IsPercentage, 
                   c.ValidFrom, c.ValidUntil, c.IsActive, c.UsageLimit, c.UsedCount, 
                   c.PlanId, c.CreatedAt, c.UpdatedAt,
                   p.Id, p.Name, p.Description, p.Price, p.IsActive, p.CompanyId, p.CreatedAt, p.UpdatedAt
            FROM Coupons c
            LEFT JOIN Plans p ON c.PlanId = p.Id
            ORDER BY c.CreatedAt DESC";

        var couponDict = new Dictionary<Guid, CouponDto>();

        await _dbConnection.QueryAsync<CouponDto, PlanDto, CouponDto>(sql,
            (coupon, plan) =>
            {
                if (!couponDict.TryGetValue(coupon.Id, out var couponEntry))
                {
                    couponEntry = coupon;
                    couponEntry.Plan = plan;
                    couponDict.Add(coupon.Id, couponEntry);
                }
                return couponEntry;
            },
            splitOn: "Id");

        return couponDict.Values;
    }

    public async Task<IEnumerable<CouponDto>> GetByPlanIdAsync(Guid planId)
    {
        const string sql = @"
            SELECT c.Id, c.Code, c.Description, c.DiscountValue, c.IsPercentage, 
                   c.ValidFrom, c.ValidUntil, c.IsActive, c.UsageLimit, c.UsedCount, 
                   c.PlanId, c.CreatedAt, c.UpdatedAt,
                   p.Id, p.Name, p.Description, p.Price, p.IsActive, p.CompanyId, p.CreatedAt, p.UpdatedAt
            FROM Coupons c
            LEFT JOIN Plans p ON c.PlanId = p.Id
            WHERE (c.PlanId = @PlanId OR c.PlanId IS NULL)
              AND c.IsActive = 1 
              AND c.ValidFrom <= GETUTCDATE() 
              AND c.ValidUntil >= GETUTCDATE()
              AND (c.UsageLimit = 0 OR c.UsedCount < c.UsageLimit)
            ORDER BY c.CreatedAt DESC";

        var couponDict = new Dictionary<Guid, CouponDto>();

        await _dbConnection.QueryAsync<CouponDto, PlanDto, CouponDto>(sql,
            (coupon, plan) =>
            {
                if (!couponDict.TryGetValue(coupon.Id, out var couponEntry))
                {
                    couponEntry = coupon;
                    couponEntry.Plan = plan;
                    couponDict.Add(coupon.Id, couponEntry);
                }
                return couponEntry;
            },
            new { PlanId = planId },
            splitOn: "Id");

        return couponDict.Values;
    }
} 
namespace Catalog.Application.Integration.Provider;

public interface ICatalogApiProvider
{
    Task<bool> PlanExistsAsync(Guid planId);
    Task<bool> CouponExistsAsync(string couponCode);
    Task<bool> IsCouponValidForPlanAsync(string couponCode, Guid planId);
    Task<PlanBasicInfo?> GetPlanBasicInfoAsync(Guid planId);
    Task<CouponBasicInfo?> GetCouponBasicInfoAsync(string couponCode);
}

public class PlanBasicInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public Guid CompanyId { get; set; }
}

public class CouponBasicInfo
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public bool IsPercentage { get; set; }
    public bool IsValid { get; set; }
    public Guid? PlanId { get; set; }
} 
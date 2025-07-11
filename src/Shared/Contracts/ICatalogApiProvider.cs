namespace Shared.Contracts;

public interface ICatalogApiProvider
{
    // Plan validations
    Task<bool> PlanExistsAsync(Guid planId);
    Task<bool> PlanExistsAsync(Guid planId, Guid companyId);
    Task<bool> IsPlanActiveAsync(Guid planId);
    Task<bool> IsPlanActiveAsync(Guid planId, Guid companyId);
    
    // Coupon validations  
    Task<bool> CouponExistsAsync(Guid couponId);
    Task<bool> CouponExistsAsync(Guid couponId, Guid companyId);
    Task<bool> IsCouponActiveAsync(Guid couponId);
    Task<bool> IsCouponActiveAsync(Guid couponId, Guid companyId);
    
    // Basic info retrieval
    Task<PlanBasicInfo?> GetPlanBasicInfoAsync(Guid planId);
    Task<PlanBasicInfo?> GetPlanBasicInfoAsync(Guid planId, Guid companyId);
    Task<CouponBasicInfo?> GetCouponBasicInfoAsync(Guid couponId);
    Task<CouponBasicInfo?> GetCouponBasicInfoAsync(Guid couponId, Guid companyId);
    
    // Business operations
    Task<decimal> CalculateDiscountAsync(Guid couponId, decimal originalAmount);
    Task<bool> IsCouponValidForPlanAsync(Guid couponId, Guid planId);
}

public class PlanBasicInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public Guid CompanyId { get; set; }
    public int DurationInDays { get; set; }
}

public class CouponBasicInfo
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
    public bool IsActive { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime? ExpiryDate { get; set; }
} 
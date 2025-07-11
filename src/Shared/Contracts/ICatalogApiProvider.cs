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

// ====================
// ADMIN API PROVIDER
// ====================

public interface IAdminApiProvider
{
    // Company validations
    Task<bool> CompanyExistsAsync(Guid companyId);
    Task<bool> IsCompanyActiveAsync(Guid companyId);
    
    // User validations  
    Task<bool> UserExistsAsync(Guid userId);
    Task<bool> UserExistsAsync(Guid userId, Guid companyId);
    Task<bool> IsUserActiveAsync(Guid userId);
    Task<bool> IsUserActiveAsync(Guid userId, Guid companyId);
    
    // Basic info retrieval
    Task<CompanyBasicInfo?> GetCompanyBasicInfoAsync(Guid companyId);
    Task<UserBasicInfo?> GetUserBasicInfoAsync(Guid userId);
    Task<UserBasicInfo?> GetUserBasicInfoAsync(Guid userId, Guid companyId);
    
    // Business operations
    Task<bool> IsUserAuthorizedForCompanyAsync(Guid userId, Guid companyId);
    Task<IEnumerable<UserBasicInfo>> GetActiveUsersByCompanyAsync(Guid companyId);
}

public class CompanyBasicInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class UserBasicInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid CompanyId { get; set; }
    public CompanyBasicInfo? Company { get; set; }
} 
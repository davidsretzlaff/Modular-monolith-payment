using Catalog.Application.Queries.Plans;
using Catalog.Domain.Repositories;
using Shared.Core.Cqrs;
using Shared.Contracts;

namespace Catalog.Application.Integration.Provider;

public class CatalogApiProvider : ICatalogApiProvider
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IPlanRepository _planRepository;
    private readonly ICouponRepository _couponRepository;

    public CatalogApiProvider(
        IQueryDispatcher queryDispatcher,
        IPlanRepository planRepository, 
        ICouponRepository couponRepository)
    {
        _queryDispatcher = queryDispatcher;
        _planRepository = planRepository;
        _couponRepository = couponRepository;
    }

    public async Task<bool> PlanExistsAsync(Guid planId)
    {
        var plan = await _planRepository.GetByIdAsync(planId);
        return plan != null;
    }

    public async Task<bool> PlanExistsAsync(Guid planId, Guid companyId)
    {
        var plan = await _planRepository.GetByIdAsync(planId, companyId);
        return plan != null;
    }

    public async Task<bool> IsPlanActiveAsync(Guid planId)
    {
        var plan = await _planRepository.GetByIdAsync(planId);
        return plan?.IsActive == true;
    }

    public async Task<bool> IsPlanActiveAsync(Guid planId, Guid companyId)
    {
        var plan = await _planRepository.GetByIdAsync(planId, companyId);
        return plan?.IsActive == true;
    }

    public async Task<bool> CouponExistsAsync(Guid couponId)
    {
        var coupon = await _couponRepository.GetByIdAsync(couponId);
        return coupon != null;
    }

    public async Task<bool> CouponExistsAsync(Guid couponId, Guid companyId)
    {
        var coupon = await _couponRepository.GetByIdAsync(couponId, companyId);
        return coupon != null;
    }

    public async Task<bool> IsCouponActiveAsync(Guid couponId)
    {
        var coupon = await _couponRepository.GetByIdAsync(couponId);
        return coupon?.IsActive == true;
    }

    public async Task<bool> IsCouponActiveAsync(Guid couponId, Guid companyId)
    {
        var coupon = await _couponRepository.GetByIdAsync(couponId, companyId);
        return coupon?.IsActive == true;
    }

    public async Task<PlanBasicInfo?> GetPlanBasicInfoAsync(Guid planId)
    {
        var planDto = await _queryDispatcher.DispatchAsync(new GetPlanByIdQuery(planId));
        if (planDto == null) return null;

        return new PlanBasicInfo
        {
            Id = planDto.Id,
            Name = planDto.Name,
            Price = planDto.Price,
            IsActive = planDto.IsActive,
            CompanyId = planDto.CompanyId,
            DurationInDays = planDto.DurationInDays
        };
    }

    public async Task<PlanBasicInfo?> GetPlanBasicInfoAsync(Guid planId, Guid companyId)
    {
        var plan = await _planRepository.GetByIdAsync(planId, companyId);
        if (plan == null) return null;

        return new PlanBasicInfo
        {
            Id = plan.Id,
            Name = plan.Name,
            Price = plan.Price,
            IsActive = plan.IsActive,
            CompanyId = plan.CompanyId,
            DurationInDays = plan.DurationInDays
        };
    }

    public async Task<CouponBasicInfo?> GetCouponBasicInfoAsync(Guid couponId)
    {
        var coupon = await _couponRepository.GetByIdAsync(couponId);
        if (coupon == null) return null;

        return new CouponBasicInfo
        {
            Id = coupon.Id,
            Code = coupon.Code,
            DiscountPercentage = coupon.DiscountPercentage,
            IsActive = coupon.IsActive,
            CompanyId = coupon.CompanyId,
            ExpiryDate = coupon.ExpiryDate
        };
    }

    public async Task<CouponBasicInfo?> GetCouponBasicInfoAsync(Guid couponId, Guid companyId)
    {
        var coupon = await _couponRepository.GetByIdAsync(couponId, companyId);
        if (coupon == null) return null;

        return new CouponBasicInfo
        {
            Id = coupon.Id,
            Code = coupon.Code,
            DiscountPercentage = coupon.DiscountPercentage,
            IsActive = coupon.IsActive,
            CompanyId = coupon.CompanyId,
            ExpiryDate = coupon.ExpiryDate
        };
    }

    public async Task<decimal> CalculateDiscountAsync(Guid couponId, decimal originalAmount)
    {
        var coupon = await _couponRepository.GetByIdAsync(couponId);
        if (coupon == null || !coupon.IsActive)
            return 0;

        return originalAmount * (coupon.DiscountPercentage / 100);
    }

    public async Task<bool> IsCouponValidForPlanAsync(Guid couponId, Guid planId)
    {
        var coupon = await _couponRepository.GetByIdAsync(couponId);
        var plan = await _planRepository.GetByIdAsync(planId);

        if (coupon == null || plan == null) 
            return false;

        if (!coupon.IsActive || !plan.IsActive)
            return false;

        if (coupon.CompanyId != plan.CompanyId)
            return false;

        if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate.Value < DateTime.UtcNow)
            return false;

        return true;
    }
} 
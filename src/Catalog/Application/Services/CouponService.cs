using Catalog.Application.Dtos;
using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;

namespace Catalog.Application.Services;

public class CouponService : ICouponService
{
    private readonly ICouponRepository _couponRepository;
    private readonly ICouponQueries _couponQueries;

    public CouponService(ICouponRepository couponRepository, ICouponQueries couponQueries)
    {
        _couponRepository = couponRepository;
        _couponQueries = couponQueries;
    }

    public async Task<CouponDto?> GetByIdAsync(Guid id)
    {
        return await _couponQueries.GetByIdAsync(id);
    }

    public async Task<CouponDto?> GetByCodeAsync(string code)
    {
        return await _couponQueries.GetByCodeAsync(code);
    }

    public async Task<IEnumerable<CouponDto>> GetActiveAsync()
    {
        return await _couponQueries.GetActiveAsync();
    }

    public async Task<IEnumerable<CouponDto>> GetAllAsync()
    {
        return await _couponQueries.GetAllAsync();
    }

    public async Task<CouponDto> CreateAsync(CreateCouponDto createDto)
    {
        if (await _couponRepository.CodeExistsAsync(createDto.Code))
            throw new InvalidOperationException($"Coupon with code '{createDto.Code}' already exists");

        var coupon = new Coupon(
            createDto.Code,
            createDto.Description,
            createDto.DiscountValue,
            createDto.IsPercentage,
            createDto.ValidFrom,
            createDto.ValidUntil,
            createDto.UsageLimit,
            createDto.PlanId
        );

        await _couponRepository.AddAsync(coupon);
        await _couponRepository.SaveChangesAsync();

        // Return the created coupon using Dapper query
        return await _couponQueries.GetByIdAsync(coupon.Id) ?? throw new InvalidOperationException("Failed to retrieve created coupon");
    }

    public async Task DeactivateAsync(Guid id)
    {
        var coupon = await _couponRepository.GetByIdAsync(id);
        if (coupon == null)
            throw new InvalidOperationException("Coupon not found");

        coupon.Deactivate();
        _couponRepository.Update(coupon);
        await _couponRepository.SaveChangesAsync();
    }

    public async Task ActivateAsync(Guid id)
    {
        var coupon = await _couponRepository.GetByIdAsync(id);
        if (coupon == null)
            throw new InvalidOperationException("Coupon not found");

        coupon.Activate();
        _couponRepository.Update(coupon);
        await _couponRepository.SaveChangesAsync();
    }

    public async Task<bool> ValidateAndUseAsync(string code)
    {
        var coupon = await _couponRepository.GetByCodeAsync(code);
        if (coupon == null || !coupon.IsValid())
            return false;

        try
        {
            coupon.Use();
            _couponRepository.Update(coupon);
            await _couponRepository.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<decimal> CalculateDiscountAsync(string code, decimal originalValue)
    {
        var coupon = await _couponRepository.GetByCodeAsync(code);
        return coupon?.CalculateDiscount(originalValue) ?? 0;
    }


} 
using Catalog.Application.Dtos;

namespace Catalog.Application.Services;

public interface ICouponService
{
    Task<CouponDto?> GetByIdAsync(Guid id);
    Task<CouponDto?> GetByCodeAsync(string code);
    Task<IEnumerable<CouponDto>> GetActiveAsync();
    Task<IEnumerable<CouponDto>> GetAllAsync();
    Task<CouponDto> CreateAsync(CreateCouponDto createDto);
    Task DeactivateAsync(Guid id);
    Task ActivateAsync(Guid id);
    Task<bool> ValidateAndUseAsync(string code);
    Task<decimal> CalculateDiscountAsync(string code, decimal originalValue);
} 
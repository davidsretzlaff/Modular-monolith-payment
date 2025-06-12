using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories;

public interface ICouponRepository
{
    Task<Coupon?> GetByIdAsync(Guid id);
    Task<Coupon?> GetByCodeAsync(string code);
    Task<IEnumerable<Coupon>> GetActiveAsync();
    Task<IEnumerable<Coupon>> GetAllAsync();
    Task AddAsync(Coupon coupon);
    void Update(Coupon coupon);
    void Delete(Coupon coupon);
    Task<bool> CodeExistsAsync(string code);
    Task SaveChangesAsync();
} 
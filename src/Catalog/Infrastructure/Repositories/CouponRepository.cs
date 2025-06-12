using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly CatalogDbContext _context;

    public CouponRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Coupon?> GetByIdAsync(Guid id)
    {
        return await _context.Coupons.FindAsync(id);
    }

    public async Task<Coupon?> GetByCodeAsync(string code)
    {
        return await _context.Coupons
            .FirstOrDefaultAsync(c => c.Code == code.ToUpperInvariant());
    }

    public async Task<IEnumerable<Coupon>> GetActiveAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Coupons
            .Where(c => c.IsActive && 
                       c.ValidFrom <= now && 
                       c.ValidUntil >= now)
            .ToListAsync();
    }

    public async Task<IEnumerable<Coupon>> GetAllAsync()
    {
        return await _context.Coupons.ToListAsync();
    }

    public async Task AddAsync(Coupon coupon)
    {
        await _context.Coupons.AddAsync(coupon);
    }

    public void Update(Coupon coupon)
    {
        _context.Coupons.Update(coupon);
    }

    public void Delete(Coupon coupon)
    {
        _context.Coupons.Remove(coupon);
    }

    public async Task<bool> CodeExistsAsync(string code)
    {
        return await _context.Coupons
            .AnyAsync(c => c.Code == code.ToUpperInvariant());
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
} 
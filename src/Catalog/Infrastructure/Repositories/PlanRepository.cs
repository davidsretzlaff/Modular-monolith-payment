using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly CatalogDbContext _context;

    public PlanRepository(CatalogDbContext context)
    {
        _context = context;
    }

    public async Task<Plan?> GetByIdAsync(Guid id)
    {
        return await _context.Plans.FindAsync(id);
    }

    public async Task<IEnumerable<Plan>> GetByCompanyIdAsync(Guid companyId)
    {
        return await _context.Plans
            .Where(p => p.CompanyId == companyId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Plan>> GetActiveAsync()
    {
        return await _context.Plans
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Plan>> GetAllAsync()
    {
        return await _context.Plans.ToListAsync();
    }

    public async Task AddAsync(Plan plan)
    {
        await _context.Plans.AddAsync(plan);
    }

    public void Update(Plan plan)
    {
        _context.Plans.Update(plan);
    }

    public void Delete(Plan plan)
    {
        _context.Plans.Remove(plan);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Plans.AnyAsync(p => p.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
} 
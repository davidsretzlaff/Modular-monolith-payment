using Admin.Domain.Entities;
using Admin.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Admin.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AdminDbContext _context;

    public CompanyRepository(AdminDbContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetByIdAsync(Guid id)
    {
        return await _context.Companies.FindAsync(id);
    }

    public async Task<IEnumerable<Company>> GetActiveAsync()
    {
        return await _context.Companies
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Company>> GetAllAsync()
    {
        return await _context.Companies
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
    }

    public void Update(Company company)
    {
        _context.Companies.Update(company);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
} 
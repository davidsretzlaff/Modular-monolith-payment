using Microsoft.EntityFrameworkCore;
using Charge.Domain.Entities;
using Charge.Domain.Repositories;

namespace Charge.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly ChargeDbContext _context;

    public SaleRepository(ChargeDbContext context)
    {
        _context = context;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, Guid companyId)
    {
        return await _context.Sales
            .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId);
    }

    public async Task<IEnumerable<Sale>> GetPendingSalesAsync()
    {
        return await _context.Sales
            .Where(x => !x.IsPaid)
            .ToListAsync();
    }

    public async Task AddAsync(Sale sale)
    {
        await _context.Sales.AddAsync(sale);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }
} 
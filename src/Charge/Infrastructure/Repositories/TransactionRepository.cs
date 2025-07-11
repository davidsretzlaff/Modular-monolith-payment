using Microsoft.EntityFrameworkCore;
using Charge.Domain.Entities;
using Charge.Domain.Repositories;

namespace Charge.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ChargeDbContext _context;

    public TransactionRepository(ChargeDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, Guid companyId)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId);
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }
} 
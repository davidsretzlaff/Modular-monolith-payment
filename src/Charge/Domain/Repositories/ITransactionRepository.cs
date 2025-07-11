using Charge.Domain.Entities;

namespace Charge.Domain.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, Guid companyId);
    Task AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
} 
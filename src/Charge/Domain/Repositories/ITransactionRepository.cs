using Modular.Charge.Domain.Entities;

namespace Modular.Charge.Domain.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, Guid companyId);
    Task AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
} 
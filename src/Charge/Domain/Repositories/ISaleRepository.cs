using Charge.Domain.Entities;

namespace Charge.Domain.Repositories;

public interface ISaleRepository
{
    Task<Sale?> GetByIdAsync(Guid id, Guid companyId);
    Task<IEnumerable<Sale>> GetPendingSalesAsync();
    Task AddAsync(Sale sale);
    Task UpdateAsync(Sale sale);
} 
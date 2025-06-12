using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories;

public interface IPlanRepository
{
    Task<Plan?> GetByIdAsync(Guid id);
    Task<IEnumerable<Plan>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<Plan>> GetActiveAsync();
    Task<IEnumerable<Plan>> GetAllAsync();
    Task AddAsync(Plan plan);
    void Update(Plan plan);
    void Delete(Plan plan);
    Task<bool> ExistsAsync(Guid id);
    Task SaveChangesAsync();
} 
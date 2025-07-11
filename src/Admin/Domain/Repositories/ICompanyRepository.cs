using Admin.Domain.Entities;

namespace Admin.Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(Guid id);
    Task<IEnumerable<Company>> GetActiveAsync();
    Task<IEnumerable<Company>> GetAllAsync();
    Task AddAsync(Company company);
    void Update(Company company);
    Task SaveChangesAsync();
} 
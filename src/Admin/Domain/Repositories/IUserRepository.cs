using Admin.Domain.Entities;

namespace Admin.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<User>> GetActiveAsync();
    Task<IEnumerable<User>> GetAllAsync();
    Task<bool> EmailExistsAsync(string email);
    Task AddAsync(User user);
    void Update(User user);
    Task SaveChangesAsync();
} 
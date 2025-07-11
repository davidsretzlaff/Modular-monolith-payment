using Checkout.Domain.Entities;

namespace Checkout.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<IEnumerable<Customer>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<Customer>> GetActiveAsync();
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<bool> EmailExistsAsync(string email);
    Task AddAsync(Customer customer);
    void Update(Customer customer);
    Task SaveChangesAsync();
} 
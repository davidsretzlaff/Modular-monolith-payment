using Checkout.Application.Dtos;

namespace Checkout.Application.Queries;

public interface ICustomerQueries
{
    Task<CustomerDto?> GetByIdAsync(Guid id);
    Task<CustomerDto?> GetByEmailAsync(string email);
    Task<IEnumerable<CustomerDto>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<CustomerDto>> GetActiveAsync();
    Task<IEnumerable<CustomerDto>> GetAllAsync();
} 
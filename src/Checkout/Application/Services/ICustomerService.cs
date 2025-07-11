using Checkout.Application.Dtos;

namespace Checkout.Application.Services;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(Guid id);
    Task<CustomerDto?> GetByEmailAsync(string email);
    Task<IEnumerable<CustomerDto>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<CustomerDto>> GetActiveAsync();
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<CustomerDto> CreateAsync(CreateCustomerDto createDto);
    Task UpdateAsync(Guid id, UpdateCustomerDto updateDto);
    Task ActivateAsync(Guid id);
    Task DeactivateAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> IsActiveAsync(Guid id);
} 
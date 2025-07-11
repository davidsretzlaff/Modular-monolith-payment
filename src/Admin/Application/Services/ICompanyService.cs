using Admin.Application.Dtos;

namespace Admin.Application.Services;

public interface ICompanyService
{
    Task<CompanyDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<CompanyDto>> GetActiveAsync();
    Task<IEnumerable<CompanyDto>> GetAllAsync();
    Task<CompanyDto> CreateAsync(CreateCompanyDto createDto);
    Task UpdateAsync(Guid id, UpdateCompanyDto updateDto);
    Task ActivateAsync(Guid id);
    Task DeactivateAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
} 
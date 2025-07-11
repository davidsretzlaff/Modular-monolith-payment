using Admin.Application.Dtos;

namespace Admin.Application.Queries;

public interface ICompanyQueries
{
    Task<CompanyDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<CompanyDto>> GetActiveAsync();
    Task<IEnumerable<CompanyDto>> GetAllAsync();
} 
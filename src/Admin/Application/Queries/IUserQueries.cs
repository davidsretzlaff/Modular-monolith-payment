using Admin.Application.Dtos;

namespace Admin.Application.Queries;

public interface IUserQueries
{
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<IEnumerable<UserDto>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<UserDto>> GetActiveAsync();
    Task<IEnumerable<UserDto>> GetAllAsync();
} 
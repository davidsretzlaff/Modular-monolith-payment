using Admin.Application.Dtos;

namespace Admin.Application.Services;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<IEnumerable<UserDto>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<UserDto>> GetActiveAsync();
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> CreateAsync(CreateUserDto createDto);
    Task UpdateAsync(Guid id, UpdateUserDto updateDto);
    Task ActivateAsync(Guid id);
    Task DeactivateAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> IsActiveAsync(Guid id);
} 
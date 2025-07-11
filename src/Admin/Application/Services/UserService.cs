using Admin.Application.Dtos;
using Admin.Application.Queries;
using Admin.Domain.Entities;
using Admin.Domain.Repositories;

namespace Admin.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserQueries _userQueries;

    public UserService(IUserRepository userRepository, IUserQueries userQueries)
    {
        _userRepository = userRepository;
        _userQueries = userQueries;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        return await _userQueries.GetByIdAsync(id);
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        return await _userQueries.GetByEmailAsync(email);
    }

    public async Task<IEnumerable<UserDto>> GetByCompanyIdAsync(Guid companyId)
    {
        return await _userQueries.GetByCompanyIdAsync(companyId);
    }

    public async Task<IEnumerable<UserDto>> GetActiveAsync()
    {
        return await _userQueries.GetActiveAsync();
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        return await _userQueries.GetAllAsync();
    }

    public async Task<UserDto> CreateAsync(CreateUserDto createDto)
    {
        if (await _userRepository.EmailExistsAsync(createDto.Email))
            throw new InvalidOperationException($"User with email '{createDto.Email}' already exists");

        var user = new User(
            createDto.Name,
            createDto.Email,
            createDto.CompanyId
        );

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return await _userQueries.GetByIdAsync(user.Id) ?? 
               throw new InvalidOperationException("Failed to retrieve created user");
    }

    public async Task UpdateAsync(Guid id, UpdateUserDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new InvalidOperationException("User not found");

        user.UpdateDetails(updateDto.Name, updateDto.Email);
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task ActivateAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new InvalidOperationException("User not found");

        user.Activate();
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task DeactivateAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new InvalidOperationException("User not found");

        user.Deactivate();
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id) != null;
    }

    public async Task<bool> IsActiveAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user?.IsActive ?? false;
    }
} 
using Admin.Application.Queries.Companies;
using Admin.Application.Queries.Users;
using Admin.Domain.Repositories;
using Shared.Core.Cqrs;
using Shared.Contracts;

namespace Admin.Application.Integration.Provider;

public class AdminApiProvider : IAdminApiProvider
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRepository _userRepository;

    public AdminApiProvider(
        IQueryDispatcher queryDispatcher,
        ICompanyRepository companyRepository,
        IUserRepository userRepository)
    {
        _queryDispatcher = queryDispatcher;
        _companyRepository = companyRepository;
        _userRepository = userRepository;
    }

    // Company validations
    public async Task<bool> CompanyExistsAsync(Guid companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);
        return company != null;
    }

    public async Task<bool> IsCompanyActiveAsync(Guid companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);
        return company?.IsActive == true;
    }

    // User validations
    public async Task<bool> UserExistsAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user != null;
    }

    public async Task<bool> UserExistsAsync(Guid userId, Guid companyId)
    {
        var user = await _userRepository.GetByIdAsync(userId, companyId);
        return user != null;
    }

    public async Task<bool> IsUserActiveAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user?.IsActive == true;
    }

    public async Task<bool> IsUserActiveAsync(Guid userId, Guid companyId)
    {
        var user = await _userRepository.GetByIdAsync(userId, companyId);
        return user?.IsActive == true;
    }

    // Basic info retrieval
    public async Task<CompanyBasicInfo?> GetCompanyBasicInfoAsync(Guid companyId)
    {
        var companyDto = await _queryDispatcher.DispatchAsync(new GetCompanyByIdQuery(companyId));
        if (companyDto == null) return null;

        return new CompanyBasicInfo
        {
            Id = companyDto.Id,
            Name = companyDto.Name,
            Email = companyDto.Email,
            IsActive = companyDto.IsActive
        };
    }

    public async Task<UserBasicInfo?> GetUserBasicInfoAsync(Guid userId)
    {
        var userDto = await _queryDispatcher.DispatchAsync(new GetUserByIdQuery(userId));
        if (userDto == null) return null;

        return new UserBasicInfo
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email,
            IsActive = userDto.IsActive,
            CompanyId = userDto.CompanyId,
            Company = userDto.Company != null ? new CompanyBasicInfo
            {
                Id = userDto.Company.Id,
                Name = userDto.Company.Name,
                Email = userDto.Company.Email,
                IsActive = userDto.Company.IsActive
            } : null
        };
    }

    public async Task<UserBasicInfo?> GetUserBasicInfoAsync(Guid userId, Guid companyId)
    {
        var user = await _userRepository.GetByIdAsync(userId, companyId);
        if (user == null) return null;

        // Get company info
        var company = await _companyRepository.GetByIdAsync(companyId);

        return new UserBasicInfo
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            IsActive = user.IsActive,
            CompanyId = user.CompanyId,
            Company = company != null ? new CompanyBasicInfo
            {
                Id = company.Id,
                Name = company.Name,
                Email = company.Email,
                IsActive = company.IsActive
            } : null
        };
    }

    // Business operations
    public async Task<bool> IsUserAuthorizedForCompanyAsync(Guid userId, Guid companyId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || !user.IsActive)
            return false;

        return user.CompanyId == companyId;
    }

    public async Task<IEnumerable<UserBasicInfo>> GetActiveUsersByCompanyAsync(Guid companyId)
    {
        var usersDto = await _queryDispatcher.DispatchAsync(new GetUsersByCompanyIdQuery(companyId));
        
        return usersDto
            .Where(u => u.IsActive)
            .Select(u => new UserBasicInfo
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                IsActive = u.IsActive,
                CompanyId = u.CompanyId,
                Company = u.Company != null ? new CompanyBasicInfo
                {
                    Id = u.Company.Id,
                    Name = u.Company.Name,
                    Email = u.Company.Email,
                    IsActive = u.Company.IsActive
                } : null
            });
    }
} 
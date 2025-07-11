using System.Net.Http;
using System.Text.Json;

namespace Admin.Application.Integration.Provider;

public class AdminApiProvider : IAdminApiProvider
{
    private readonly HttpClient _httpClient;

    public AdminApiProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CompanyExistsAsync(Guid companyId)
    {
        // TODO: Implement actual API call to Admin service
        // For now, return true to allow development
        return true;
    }

    public async Task<bool> UserExistsAsync(Guid userId)
    {
        // TODO: Implement actual API call to Admin service
        return true;
    }

    public async Task<bool> IsUserActiveAsync(Guid userId)
    {
        // TODO: Implement actual API call to Admin service
        return true;
    }

    public async Task<CompanyBasicInfo?> GetCompanyBasicInfoAsync(Guid companyId)
    {
        // TODO: Implement actual API call to Admin service
        return new CompanyBasicInfo
        {
            Id = companyId,
            Name = "Default Company",
            IsActive = true
        };
    }
} 
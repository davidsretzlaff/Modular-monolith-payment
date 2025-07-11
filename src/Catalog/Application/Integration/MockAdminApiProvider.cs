using System.Net.Http;

namespace Catalog.Application.Integration;

public class MockAdminApiProvider : IAdminApiProvider
{
    private readonly HttpClient _httpClient;

    public MockAdminApiProvider(HttpClient httpClient)
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
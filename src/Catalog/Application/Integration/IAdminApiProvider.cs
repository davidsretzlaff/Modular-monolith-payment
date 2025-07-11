namespace Catalog.Application.Integration;

public interface IAdminApiProvider
{
    Task<bool> CompanyExistsAsync(Guid companyId);
    Task<bool> UserExistsAsync(Guid userId);
    Task<bool> IsUserActiveAsync(Guid userId);
    Task<CompanyBasicInfo?> GetCompanyBasicInfoAsync(Guid companyId);
}

public class CompanyBasicInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
} 
using Catalog.Application.Dtos;
using Dapper;
using System.Data;

namespace Catalog.Application.Queries;

public interface IPlanQueries
{
    Task<PlanDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<PlanDto>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<PlanDto>> GetActiveAsync();
    Task<IEnumerable<PlanDto>> GetAllAsync();
}

public class PlanQueries : IPlanQueries
{
    private readonly IDbConnection _dbConnection;

    public PlanQueries(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<PlanDto?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT p.Id, p.Name, p.Description, p.Price, p.IsActive, 
                   p.CompanyId, p.DurationInDays, p.CreatedAt, p.UpdatedAt,
                   c.Id, c.Name, c.IsActive, c.CreatedAt, c.UpdatedAt
            FROM Plans p
            LEFT JOIN Companies c ON p.CompanyId = c.Id
            WHERE p.Id = @Id";

        var planDict = new Dictionary<Guid, PlanDto>();

        await _dbConnection.QueryAsync<PlanDto, CompanyDto, PlanDto>(sql,
            (plan, company) =>
            {
                if (!planDict.TryGetValue(plan.Id, out var planEntry))
                {
                    planEntry = plan;
                    planEntry.Company = company;
                    planDict.Add(plan.Id, planEntry);
                }
                return planEntry;
            },
            new { Id = id },
            splitOn: "Id");

        return planDict.Values.FirstOrDefault();
    }

    public async Task<IEnumerable<PlanDto>> GetByCompanyIdAsync(Guid companyId)
    {
        const string sql = @"
            SELECT p.Id, p.Name, p.Description, p.Price, p.IsActive, 
                   p.CompanyId, p.DurationInDays, p.CreatedAt, p.UpdatedAt,
                   c.Id, c.Name, c.IsActive, c.CreatedAt, c.UpdatedAt
            FROM Plans p
            LEFT JOIN Companies c ON p.CompanyId = c.Id
            WHERE p.CompanyId = @CompanyId
            ORDER BY p.CreatedAt DESC";

        var planDict = new Dictionary<Guid, PlanDto>();

        await _dbConnection.QueryAsync<PlanDto, CompanyDto, PlanDto>(sql,
            (plan, company) =>
            {
                if (!planDict.TryGetValue(plan.Id, out var planEntry))
                {
                    planEntry = plan;
                    planEntry.Company = company;
                    planDict.Add(plan.Id, planEntry);
                }
                return planEntry;
            },
            new { CompanyId = companyId },
            splitOn: "Id");

        return planDict.Values;
    }

    public async Task<IEnumerable<PlanDto>> GetActiveAsync()
    {
        const string sql = @"
            SELECT p.Id, p.Name, p.Description, p.Price, p.IsActive, 
                   p.CompanyId, p.DurationInDays, p.CreatedAt, p.UpdatedAt,
                   c.Id, c.Name, c.IsActive, c.CreatedAt, c.UpdatedAt
            FROM Plans p
            LEFT JOIN Companies c ON p.CompanyId = c.Id
            WHERE p.IsActive = 1
            ORDER BY p.CreatedAt DESC";

        var planDict = new Dictionary<Guid, PlanDto>();

        await _dbConnection.QueryAsync<PlanDto, CompanyDto, PlanDto>(sql,
            (plan, company) =>
            {
                if (!planDict.TryGetValue(plan.Id, out var planEntry))
                {
                    planEntry = plan;
                    planEntry.Company = company;
                    planDict.Add(plan.Id, planEntry);
                }
                return planEntry;
            },
            splitOn: "Id");

        return planDict.Values;
    }

    public async Task<IEnumerable<PlanDto>> GetAllAsync()
    {
        const string sql = @"
            SELECT p.Id, p.Name, p.Description, p.Price, p.IsActive, 
                   p.CompanyId, p.DurationInDays, p.CreatedAt, p.UpdatedAt,
                   c.Id, c.Name, c.IsActive, c.CreatedAt, c.UpdatedAt
            FROM Plans p
            LEFT JOIN Companies c ON p.CompanyId = c.Id
            ORDER BY p.CreatedAt DESC";

        var planDict = new Dictionary<Guid, PlanDto>();

        await _dbConnection.QueryAsync<PlanDto, CompanyDto, PlanDto>(sql,
            (plan, company) =>
            {
                if (!planDict.TryGetValue(plan.Id, out var planEntry))
                {
                    planEntry = plan;
                    planEntry.Company = company;
                    planDict.Add(plan.Id, planEntry);
                }
                return planEntry;
            },
            splitOn: "Id");

        return planDict.Values;
    }
} 
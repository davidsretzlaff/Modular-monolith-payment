using Catalog.Application.Dtos;
using Dapper;
using Shared.Core.Cqrs;
using System.Data;

namespace Catalog.Application.Queries.Plans;

public class GetActivePlansQueryHandler : IQueryHandler<GetActivePlansQuery, IEnumerable<PlanDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetActivePlansQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<PlanDto>> HandleAsync(GetActivePlansQuery query)
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
}

public class GetAllPlansQueryHandler : IQueryHandler<GetAllPlansQuery, IEnumerable<PlanDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetAllPlansQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<PlanDto>> HandleAsync(GetAllPlansQuery query)
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
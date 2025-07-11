using Admin.Application.Dtos;
using Dapper;
using Shared.Core.Cqrs;
using System.Data;

namespace Admin.Application.Queries.Companies;

public class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery, CompanyDto?>
{
    private readonly IDbConnection _dbConnection;

    public GetCompanyByIdQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<CompanyDto?> HandleAsync(GetCompanyByIdQuery query)
    {
        const string sql = @"
            SELECT Id, Name, Email, IsActive, CreatedAt, UpdatedAt
            FROM Companies
            WHERE Id = @Id";

        return await _dbConnection.QuerySingleOrDefaultAsync<CompanyDto>(sql, new { Id = query.Id });
    }
}

public class GetAllCompaniesQueryHandler : IQueryHandler<GetAllCompaniesQuery, IEnumerable<CompanyDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetAllCompaniesQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<CompanyDto>> HandleAsync(GetAllCompaniesQuery query)
    {
        const string sql = @"
            SELECT Id, Name, Email, IsActive, CreatedAt, UpdatedAt
            FROM Companies
            ORDER BY CreatedAt DESC";

        return await _dbConnection.QueryAsync<CompanyDto>(sql);
    }
}

public class GetActiveCompaniesQueryHandler : IQueryHandler<GetActiveCompaniesQuery, IEnumerable<CompanyDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetActiveCompaniesQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<CompanyDto>> HandleAsync(GetActiveCompaniesQuery query)
    {
        const string sql = @"
            SELECT Id, Name, Email, IsActive, CreatedAt, UpdatedAt
            FROM Companies
            WHERE IsActive = 1
            ORDER BY CreatedAt DESC";

        return await _dbConnection.QueryAsync<CompanyDto>(sql);
    }
} 
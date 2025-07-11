using Admin.Application.Dtos;
using Dapper;
using System.Data;

namespace Admin.Application.Queries;

public class CompanyQueries : ICompanyQueries
{
    private readonly IDbConnection _dbConnection;

    public CompanyQueries(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<CompanyDto?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT Id, Name, Email, Phone, IsActive, CreatedAt, UpdatedAt
            FROM Companies
            WHERE Id = @Id";

        return await _dbConnection.QuerySingleOrDefaultAsync<CompanyDto>(sql, new { Id = id });
    }

    public async Task<IEnumerable<CompanyDto>> GetActiveAsync()
    {
        const string sql = @"
            SELECT Id, Name, Email, Phone, IsActive, CreatedAt, UpdatedAt
            FROM Companies
            WHERE IsActive = 1
            ORDER BY CreatedAt DESC";

        return await _dbConnection.QueryAsync<CompanyDto>(sql);
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        const string sql = @"
            SELECT Id, Name, Email, Phone, IsActive, CreatedAt, UpdatedAt
            FROM Companies
            ORDER BY CreatedAt DESC";

        return await _dbConnection.QueryAsync<CompanyDto>(sql);
    }
} 
using Admin.Application.Dtos;
using Dapper;
using Shared.Core.Cqrs;
using System.Data;

namespace Admin.Application.Queries.Users;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IDbConnection _dbConnection;

    public GetUserByIdQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<UserDto?> HandleAsync(GetUserByIdQuery query)
    {
        const string sql = @"
            SELECT u.Id, u.Name, u.Email, u.IsActive, u.CompanyId, u.CreatedAt, u.UpdatedAt,
                   c.Id, c.Name, c.Email, c.IsActive, c.CreatedAt, c.UpdatedAt
            FROM Users u
            LEFT JOIN Companies c ON u.CompanyId = c.Id
            WHERE u.Id = @Id";

        var userDict = new Dictionary<Guid, UserDto>();

        await _dbConnection.QueryAsync<UserDto, CompanyDto, UserDto>(sql,
            (user, company) =>
            {
                if (!userDict.TryGetValue(user.Id, out var userEntry))
                {
                    userEntry = user;
                    userEntry.Company = company;
                    userDict.Add(user.Id, userEntry);
                }
                return userEntry;
            },
            new { Id = query.Id },
            splitOn: "Id");

        return userDict.Values.FirstOrDefault();
    }
}

public class GetUsersByCompanyIdQueryHandler : IQueryHandler<GetUsersByCompanyIdQuery, IEnumerable<UserDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetUsersByCompanyIdQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<UserDto>> HandleAsync(GetUsersByCompanyIdQuery query)
    {
        const string sql = @"
            SELECT u.Id, u.Name, u.Email, u.IsActive, u.CompanyId, u.CreatedAt, u.UpdatedAt,
                   c.Id, c.Name, c.Email, c.IsActive, c.CreatedAt, c.UpdatedAt
            FROM Users u
            LEFT JOIN Companies c ON u.CompanyId = c.Id
            WHERE u.CompanyId = @CompanyId
            ORDER BY u.CreatedAt DESC";

        var userDict = new Dictionary<Guid, UserDto>();

        await _dbConnection.QueryAsync<UserDto, CompanyDto, UserDto>(sql,
            (user, company) =>
            {
                if (!userDict.TryGetValue(user.Id, out var userEntry))
                {
                    userEntry = user;
                    userEntry.Company = company;
                    userDict.Add(user.Id, userEntry);
                }
                return userEntry;
            },
            new { CompanyId = query.CompanyId },
            splitOn: "Id");

        return userDict.Values;
    }
}

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetAllUsersQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<UserDto>> HandleAsync(GetAllUsersQuery query)
    {
        const string sql = @"
            SELECT u.Id, u.Name, u.Email, u.IsActive, u.CompanyId, u.CreatedAt, u.UpdatedAt,
                   c.Id, c.Name, c.Email, c.IsActive, c.CreatedAt, c.UpdatedAt
            FROM Users u
            LEFT JOIN Companies c ON u.CompanyId = c.Id
            ORDER BY u.CreatedAt DESC";

        var userDict = new Dictionary<Guid, UserDto>();

        await _dbConnection.QueryAsync<UserDto, CompanyDto, UserDto>(sql,
            (user, company) =>
            {
                if (!userDict.TryGetValue(user.Id, out var userEntry))
                {
                    userEntry = user;
                    userEntry.Company = company;
                    userDict.Add(user.Id, userEntry);
                }
                return userEntry;
            },
            splitOn: "Id");

        return userDict.Values;
    }
}

public class GetActiveUsersQueryHandler : IQueryHandler<GetActiveUsersQuery, IEnumerable<UserDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetActiveUsersQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<UserDto>> HandleAsync(GetActiveUsersQuery query)
    {
        const string sql = @"
            SELECT u.Id, u.Name, u.Email, u.IsActive, u.CompanyId, u.CreatedAt, u.UpdatedAt,
                   c.Id, c.Name, c.Email, c.IsActive, c.CreatedAt, c.UpdatedAt
            FROM Users u
            LEFT JOIN Companies c ON u.CompanyId = c.Id
            WHERE u.IsActive = 1
            ORDER BY u.CreatedAt DESC";

        var userDict = new Dictionary<Guid, UserDto>();

        await _dbConnection.QueryAsync<UserDto, CompanyDto, UserDto>(sql,
            (user, company) =>
            {
                if (!userDict.TryGetValue(user.Id, out var userEntry))
                {
                    userEntry = user;
                    userEntry.Company = company;
                    userDict.Add(user.Id, userEntry);
                }
                return userEntry;
            },
            splitOn: "Id");

        return userDict.Values;
    }
} 
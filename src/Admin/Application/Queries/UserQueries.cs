using Admin.Application.Dtos;
using Dapper;
using System.Data;

namespace Admin.Application.Queries;

public class UserQueries : IUserQueries
{
    private readonly IDbConnection _dbConnection;

    public UserQueries(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT u.Id, u.Name, u.Email, u.CompanyId, u.IsActive, u.CreatedAt, u.UpdatedAt,
                   c.Id, c.Name, c.Email, c.Phone, c.IsActive, c.CreatedAt, c.UpdatedAt
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
            new { Id = id },
            splitOn: "Id");

        return userDict.Values.FirstOrDefault();
    }

    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        const string sql = @"
            SELECT u.Id, u.Name, u.Email, u.CompanyId, u.IsActive, u.CreatedAt, u.UpdatedAt,
                   c.Id, c.Name, c.Email, c.Phone, c.IsActive, c.CreatedAt, c.UpdatedAt
            FROM Users u
            LEFT JOIN Companies c ON u.CompanyId = c.Id
            WHERE u.Email = @Email";

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
            new { Email = email },
            splitOn: "Id");

        return userDict.Values.FirstOrDefault();
    }

    public async Task<IEnumerable<UserDto>> GetByCompanyIdAsync(Guid companyId)
    {
        const string sql = @"
            SELECT u.Id, u.Name, u.Email, u.CompanyId, u.IsActive, u.CreatedAt, u.UpdatedAt,
                   c.Id, c.Name, c.Email, c.Phone, c.IsActive, c.CreatedAt, c.UpdatedAt
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
            new { CompanyId = companyId },
            splitOn: "Id");

        return userDict.Values;
    }

    public async Task<IEnumerable<UserDto>> GetActiveAsync()
    {
        const string sql = @"
            SELECT u.Id, u.Name, u.Email, u.CompanyId, u.IsActive, u.CreatedAt, u.UpdatedAt,
                   c.Id, c.Name, c.Email, c.Phone, c.IsActive, c.CreatedAt, c.UpdatedAt
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

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        const string sql = @"
            SELECT u.Id, u.Name, u.Email, u.CompanyId, u.IsActive, u.CreatedAt, u.UpdatedAt,
                   c.Id, c.Name, c.Email, c.Phone, c.IsActive, c.CreatedAt, c.UpdatedAt
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
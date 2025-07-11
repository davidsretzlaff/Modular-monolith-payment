using Checkout.Application.Dtos;
using Dapper;
using System.Data;

namespace Checkout.Application.Queries;

public class CustomerQueries : ICustomerQueries
{
    private readonly IDbConnection _dbConnection;

    public CustomerQueries(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT c.Id, c.Name, c.Email, c.CompanyId, c.IsActive, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.Phone, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Customers c
            LEFT JOIN Companies co ON c.CompanyId = co.Id
            WHERE c.Id = @Id";

        var customerDict = new Dictionary<Guid, CustomerDto>();

        await _dbConnection.QueryAsync<CustomerDto, CompanyDto, CustomerDto>(sql,
            (customer, company) =>
            {
                if (!customerDict.TryGetValue(customer.Id, out var customerEntry))
                {
                    customerEntry = customer;
                    customerEntry.Company = company;
                    customerDict.Add(customer.Id, customerEntry);
                }
                return customerEntry;
            },
            new { Id = id },
            splitOn: "Id");

        return customerDict.Values.FirstOrDefault();
    }

    public async Task<CustomerDto?> GetByEmailAsync(string email)
    {
        const string sql = @"
            SELECT c.Id, c.Name, c.Email, c.CompanyId, c.IsActive, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.Phone, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Customers c
            LEFT JOIN Companies co ON c.CompanyId = co.Id
            WHERE c.Email = @Email";

        var customerDict = new Dictionary<Guid, CustomerDto>();

        await _dbConnection.QueryAsync<CustomerDto, CompanyDto, CustomerDto>(sql,
            (customer, company) =>
            {
                if (!customerDict.TryGetValue(customer.Id, out var customerEntry))
                {
                    customerEntry = customer;
                    customerEntry.Company = company;
                    customerDict.Add(customer.Id, customerEntry);
                }
                return customerEntry;
            },
            new { Email = email },
            splitOn: "Id");

        return customerDict.Values.FirstOrDefault();
    }

    public async Task<IEnumerable<CustomerDto>> GetByCompanyIdAsync(Guid companyId)
    {
        const string sql = @"
            SELECT c.Id, c.Name, c.Email, c.CompanyId, c.IsActive, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.Phone, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Customers c
            LEFT JOIN Companies co ON c.CompanyId = co.Id
            WHERE c.CompanyId = @CompanyId
            ORDER BY c.CreatedAt DESC";

        var customerDict = new Dictionary<Guid, CustomerDto>();

        await _dbConnection.QueryAsync<CustomerDto, CompanyDto, CustomerDto>(sql,
            (customer, company) =>
            {
                if (!customerDict.TryGetValue(customer.Id, out var customerEntry))
                {
                    customerEntry = customer;
                    customerEntry.Company = company;
                    customerDict.Add(customer.Id, customerEntry);
                }
                return customerEntry;
            },
            new { CompanyId = companyId },
            splitOn: "Id");

        return customerDict.Values;
    }

    public async Task<IEnumerable<CustomerDto>> GetActiveAsync()
    {
        const string sql = @"
            SELECT c.Id, c.Name, c.Email, c.CompanyId, c.IsActive, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.Phone, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Customers c
            LEFT JOIN Companies co ON c.CompanyId = co.Id
            WHERE c.IsActive = 1
            ORDER BY c.CreatedAt DESC";

        var customerDict = new Dictionary<Guid, CustomerDto>();

        await _dbConnection.QueryAsync<CustomerDto, CompanyDto, CustomerDto>(sql,
            (customer, company) =>
            {
                if (!customerDict.TryGetValue(customer.Id, out var customerEntry))
                {
                    customerEntry = customer;
                    customerEntry.Company = company;
                    customerDict.Add(customer.Id, customerEntry);
                }
                return customerEntry;
            },
            splitOn: "Id");

        return customerDict.Values;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        const string sql = @"
            SELECT c.Id, c.Name, c.Email, c.CompanyId, c.IsActive, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.Phone, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Customers c
            LEFT JOIN Companies co ON c.CompanyId = co.Id
            ORDER BY c.CreatedAt DESC";

        var customerDict = new Dictionary<Guid, CustomerDto>();

        await _dbConnection.QueryAsync<CustomerDto, CompanyDto, CustomerDto>(sql,
            (customer, company) =>
            {
                if (!customerDict.TryGetValue(customer.Id, out var customerEntry))
                {
                    customerEntry = customer;
                    customerEntry.Company = company;
                    customerDict.Add(customer.Id, customerEntry);
                }
                return customerEntry;
            },
            splitOn: "Id");

        return customerDict.Values;
    }
} 
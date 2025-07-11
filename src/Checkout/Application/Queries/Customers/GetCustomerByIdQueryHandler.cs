using Checkout.Application.Dtos;
using Dapper;
using Shared.Core.Cqrs;
using System.Data;

namespace Checkout.Application.Queries.Customers;

public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly IDbConnection _dbConnection;

    public GetCustomerByIdQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<CustomerDto?> HandleAsync(GetCustomerByIdQuery query)
    {
        const string sql = @"
            SELECT c.Id, c.Name, c.Email, c.IsActive, c.CompanyId, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Customers c
            LEFT JOIN Companies co ON c.CompanyId = co.Id
            WHERE c.Id = @Id AND c.CompanyId = @CompanyId";

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
            new { Id = query.Id, CompanyId = query.CompanyId },
            splitOn: "Id");

        return customerDict.Values.FirstOrDefault();
    }
}

public class GetCustomersByCompanyIdQueryHandler : IQueryHandler<GetCustomersByCompanyIdQuery, IEnumerable<CustomerDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetCustomersByCompanyIdQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<CustomerDto>> HandleAsync(GetCustomersByCompanyIdQuery query)
    {
        const string sql = @"
            SELECT c.Id, c.Name, c.Email, c.IsActive, c.CompanyId, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.IsActive, co.CreatedAt, co.UpdatedAt
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
            new { CompanyId = query.CompanyId },
            splitOn: "Id");

        return customerDict.Values;
    }
}

public class GetActiveCustomersQueryHandler : IQueryHandler<GetActiveCustomersQuery, IEnumerable<CustomerDto>>
{
    private readonly IDbConnection _dbConnection;

    public GetActiveCustomersQueryHandler(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<IEnumerable<CustomerDto>> HandleAsync(GetActiveCustomersQuery query)
    {
        const string sql = @"
            SELECT c.Id, c.Name, c.Email, c.IsActive, c.CompanyId, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Customers c
            LEFT JOIN Companies co ON c.CompanyId = co.Id
            WHERE c.CompanyId = @CompanyId AND c.IsActive = 1
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
            new { CompanyId = query.CompanyId },
            splitOn: "Id");

        return customerDict.Values;
    }
} 
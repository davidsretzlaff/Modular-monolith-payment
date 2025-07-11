using Charge.Application.Dtos;
using Dapper;
using System.Data;

namespace Charge.Application.Queries;

public class SaleQueries : ISaleQueries
{
    private readonly IDbConnection _dbConnection;

    public SaleQueries(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<SaleDto?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT s.Id, s.CustomerId, s.CompanyId, s.TotalAmount, s.Status, 
                   s.Description, s.IsActive, s.CreatedAt, s.UpdatedAt,
                   c.Id, c.Name, c.Email, c.CompanyId, c.IsActive, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.Phone, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Sales s
            LEFT JOIN Customers c ON s.CustomerId = c.Id
            LEFT JOIN Companies co ON s.CompanyId = co.Id
            WHERE s.Id = @Id";

        var saleDict = new Dictionary<Guid, SaleDto>();

        await _dbConnection.QueryAsync<SaleDto, CustomerDto, CompanyDto, SaleDto>(sql,
            (sale, customer, company) =>
            {
                if (!saleDict.TryGetValue(sale.Id, out var saleEntry))
                {
                    saleEntry = sale;
                    saleEntry.Customer = customer;
                    saleEntry.Company = company;
                    saleDict.Add(sale.Id, saleEntry);
                }
                return saleEntry;
            },
            new { Id = id },
            splitOn: "Id");

        return saleDict.Values.FirstOrDefault();
    }

    public async Task<IEnumerable<SaleDto>> GetByCustomerIdAsync(Guid customerId)
    {
        const string sql = @"
            SELECT s.Id, s.CustomerId, s.CompanyId, s.TotalAmount, s.Status, 
                   s.Description, s.IsActive, s.CreatedAt, s.UpdatedAt,
                   c.Id, c.Name, c.Email, c.CompanyId, c.IsActive, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.Phone, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Sales s
            LEFT JOIN Customers c ON s.CustomerId = c.Id
            LEFT JOIN Companies co ON s.CompanyId = co.Id
            WHERE s.CustomerId = @CustomerId
            ORDER BY s.CreatedAt DESC";

        var saleDict = new Dictionary<Guid, SaleDto>();

        await _dbConnection.QueryAsync<SaleDto, CustomerDto, CompanyDto, SaleDto>(sql,
            (sale, customer, company) =>
            {
                if (!saleDict.TryGetValue(sale.Id, out var saleEntry))
                {
                    saleEntry = sale;
                    saleEntry.Customer = customer;
                    saleEntry.Company = company;
                    saleDict.Add(sale.Id, saleEntry);
                }
                return saleEntry;
            },
            new { CustomerId = customerId },
            splitOn: "Id");

        return saleDict.Values;
    }

    public async Task<IEnumerable<SaleDto>> GetByCompanyIdAsync(Guid companyId)
    {
        const string sql = @"
            SELECT s.Id, s.CustomerId, s.CompanyId, s.TotalAmount, s.Status, 
                   s.Description, s.IsActive, s.CreatedAt, s.UpdatedAt,
                   c.Id, c.Name, c.Email, c.CompanyId, c.IsActive, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.Phone, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Sales s
            LEFT JOIN Customers c ON s.CustomerId = c.Id
            LEFT JOIN Companies co ON s.CompanyId = co.Id
            WHERE s.CompanyId = @CompanyId
            ORDER BY s.CreatedAt DESC";

        var saleDict = new Dictionary<Guid, SaleDto>();

        await _dbConnection.QueryAsync<SaleDto, CustomerDto, CompanyDto, SaleDto>(sql,
            (sale, customer, company) =>
            {
                if (!saleDict.TryGetValue(sale.Id, out var saleEntry))
                {
                    saleEntry = sale;
                    saleEntry.Customer = customer;
                    saleEntry.Company = company;
                    saleDict.Add(sale.Id, saleEntry);
                }
                return saleEntry;
            },
            new { CompanyId = companyId },
            splitOn: "Id");

        return saleDict.Values;
    }

    public async Task<IEnumerable<SaleDto>> GetActiveAsync()
    {
        const string sql = @"
            SELECT s.Id, s.CustomerId, s.CompanyId, s.TotalAmount, s.Status, 
                   s.Description, s.IsActive, s.CreatedAt, s.UpdatedAt,
                   c.Id, c.Name, c.Email, c.CompanyId, c.IsActive, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.Phone, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Sales s
            LEFT JOIN Customers c ON s.CustomerId = c.Id
            LEFT JOIN Companies co ON s.CompanyId = co.Id
            WHERE s.IsActive = 1
            ORDER BY s.CreatedAt DESC";

        var saleDict = new Dictionary<Guid, SaleDto>();

        await _dbConnection.QueryAsync<SaleDto, CustomerDto, CompanyDto, SaleDto>(sql,
            (sale, customer, company) =>
            {
                if (!saleDict.TryGetValue(sale.Id, out var saleEntry))
                {
                    saleEntry = sale;
                    saleEntry.Customer = customer;
                    saleEntry.Company = company;
                    saleDict.Add(sale.Id, saleEntry);
                }
                return saleEntry;
            },
            splitOn: "Id");

        return saleDict.Values;
    }

    public async Task<IEnumerable<SaleDto>> GetAllAsync()
    {
        const string sql = @"
            SELECT s.Id, s.CustomerId, s.CompanyId, s.TotalAmount, s.Status, 
                   s.Description, s.IsActive, s.CreatedAt, s.UpdatedAt,
                   c.Id, c.Name, c.Email, c.CompanyId, c.IsActive, c.CreatedAt, c.UpdatedAt,
                   co.Id, co.Name, co.Email, co.Phone, co.IsActive, co.CreatedAt, co.UpdatedAt
            FROM Sales s
            LEFT JOIN Customers c ON s.CustomerId = c.Id
            LEFT JOIN Companies co ON s.CompanyId = co.Id
            ORDER BY s.CreatedAt DESC";

        var saleDict = new Dictionary<Guid, SaleDto>();

        await _dbConnection.QueryAsync<SaleDto, CustomerDto, CompanyDto, SaleDto>(sql,
            (sale, customer, company) =>
            {
                if (!saleDict.TryGetValue(sale.Id, out var saleEntry))
                {
                    saleEntry = sale;
                    saleEntry.Customer = customer;
                    saleEntry.Company = company;
                    saleDict.Add(sale.Id, saleEntry);
                }
                return saleEntry;
            },
            splitOn: "Id");

        return saleDict.Values;
    }

    public async Task<decimal> GetTotalRevenueAsync(Guid companyId, DateTime startDate, DateTime endDate)
    {
        const string sql = @"
            SELECT COALESCE(SUM(TotalAmount), 0)
            FROM Sales
            WHERE CompanyId = @CompanyId 
              AND CreatedAt >= @StartDate 
              AND CreatedAt <= @EndDate
              AND IsActive = 1";

        return await _dbConnection.QuerySingleAsync<decimal>(sql, new { CompanyId = companyId, StartDate = startDate, EndDate = endDate });
    }
} 
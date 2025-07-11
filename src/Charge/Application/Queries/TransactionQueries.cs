using Charge.Application.Dtos;
using Dapper;
using System.Data;

namespace Charge.Application.Queries;

public class TransactionQueries : ITransactionQueries
{
    private readonly IDbConnection _dbConnection;

    public TransactionQueries(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<TransactionDto?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT t.Id, t.SaleId, t.Amount, t.PaymentMethod, t.Status, 
                   t.IsSuccessful, t.CreatedAt, t.UpdatedAt,
                   s.Id, s.CustomerId, s.CompanyId, s.TotalAmount, s.Status, 
                   s.Description, s.IsActive, s.CreatedAt, s.UpdatedAt
            FROM Transactions t
            LEFT JOIN Sales s ON t.SaleId = s.Id
            WHERE t.Id = @Id";

        var transactionDict = new Dictionary<Guid, TransactionDto>();

        await _dbConnection.QueryAsync<TransactionDto, SaleDto, TransactionDto>(sql,
            (transaction, sale) =>
            {
                if (!transactionDict.TryGetValue(transaction.Id, out var transactionEntry))
                {
                    transactionEntry = transaction;
                    transactionEntry.Sale = sale;
                    transactionDict.Add(transaction.Id, transactionEntry);
                }
                return transactionEntry;
            },
            new { Id = id },
            splitOn: "Id");

        return transactionDict.Values.FirstOrDefault();
    }

    public async Task<IEnumerable<TransactionDto>> GetBySaleIdAsync(Guid saleId)
    {
        const string sql = @"
            SELECT t.Id, t.SaleId, t.Amount, t.PaymentMethod, t.Status, 
                   t.IsSuccessful, t.CreatedAt, t.UpdatedAt,
                   s.Id, s.CustomerId, s.CompanyId, s.TotalAmount, s.Status, 
                   s.Description, s.IsActive, s.CreatedAt, s.UpdatedAt
            FROM Transactions t
            LEFT JOIN Sales s ON t.SaleId = s.Id
            WHERE t.SaleId = @SaleId
            ORDER BY t.CreatedAt DESC";

        var transactionDict = new Dictionary<Guid, TransactionDto>();

        await _dbConnection.QueryAsync<TransactionDto, SaleDto, TransactionDto>(sql,
            (transaction, sale) =>
            {
                if (!transactionDict.TryGetValue(transaction.Id, out var transactionEntry))
                {
                    transactionEntry = transaction;
                    transactionEntry.Sale = sale;
                    transactionDict.Add(transaction.Id, transactionEntry);
                }
                return transactionEntry;
            },
            new { SaleId = saleId },
            splitOn: "Id");

        return transactionDict.Values;
    }

    public async Task<IEnumerable<TransactionDto>> GetByCustomerIdAsync(Guid customerId)
    {
        const string sql = @"
            SELECT t.Id, t.SaleId, t.Amount, t.PaymentMethod, t.Status, 
                   t.IsSuccessful, t.CreatedAt, t.UpdatedAt,
                   s.Id, s.CustomerId, s.CompanyId, s.TotalAmount, s.Status, 
                   s.Description, s.IsActive, s.CreatedAt, s.UpdatedAt
            FROM Transactions t
            LEFT JOIN Sales s ON t.SaleId = s.Id
            WHERE s.CustomerId = @CustomerId
            ORDER BY t.CreatedAt DESC";

        var transactionDict = new Dictionary<Guid, TransactionDto>();

        await _dbConnection.QueryAsync<TransactionDto, SaleDto, TransactionDto>(sql,
            (transaction, sale) =>
            {
                if (!transactionDict.TryGetValue(transaction.Id, out var transactionEntry))
                {
                    transactionEntry = transaction;
                    transactionEntry.Sale = sale;
                    transactionDict.Add(transaction.Id, transactionEntry);
                }
                return transactionEntry;
            },
            new { CustomerId = customerId },
            splitOn: "Id");

        return transactionDict.Values;
    }

    public async Task<IEnumerable<TransactionDto>> GetByStatusAsync(string status)
    {
        const string sql = @"
            SELECT t.Id, t.SaleId, t.Amount, t.PaymentMethod, t.Status, 
                   t.IsSuccessful, t.CreatedAt, t.UpdatedAt,
                   s.Id, s.CustomerId, s.CompanyId, s.TotalAmount, s.Status, 
                   s.Description, s.IsActive, s.CreatedAt, s.UpdatedAt
            FROM Transactions t
            LEFT JOIN Sales s ON t.SaleId = s.Id
            WHERE t.Status = @Status
            ORDER BY t.CreatedAt DESC";

        var transactionDict = new Dictionary<Guid, TransactionDto>();

        await _dbConnection.QueryAsync<TransactionDto, SaleDto, TransactionDto>(sql,
            (transaction, sale) =>
            {
                if (!transactionDict.TryGetValue(transaction.Id, out var transactionEntry))
                {
                    transactionEntry = transaction;
                    transactionEntry.Sale = sale;
                    transactionDict.Add(transaction.Id, transactionEntry);
                }
                return transactionEntry;
            },
            new { Status = status },
            splitOn: "Id");

        return transactionDict.Values;
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync()
    {
        const string sql = @"
            SELECT t.Id, t.SaleId, t.Amount, t.PaymentMethod, t.Status, 
                   t.IsSuccessful, t.CreatedAt, t.UpdatedAt,
                   s.Id, s.CustomerId, s.CompanyId, s.TotalAmount, s.Status, 
                   s.Description, s.IsActive, s.CreatedAt, s.UpdatedAt
            FROM Transactions t
            LEFT JOIN Sales s ON t.SaleId = s.Id
            ORDER BY t.CreatedAt DESC";

        var transactionDict = new Dictionary<Guid, TransactionDto>();

        await _dbConnection.QueryAsync<TransactionDto, SaleDto, TransactionDto>(sql,
            (transaction, sale) =>
            {
                if (!transactionDict.TryGetValue(transaction.Id, out var transactionEntry))
                {
                    transactionEntry = transaction;
                    transactionEntry.Sale = sale;
                    transactionDict.Add(transaction.Id, transactionEntry);
                }
                return transactionEntry;
            },
            splitOn: "Id");

        return transactionDict.Values;
    }

    public async Task<decimal> GetTotalTransactionsAsync(Guid companyId, DateTime startDate, DateTime endDate)
    {
        const string sql = @"
            SELECT COALESCE(SUM(t.Amount), 0)
            FROM Transactions t
            INNER JOIN Sales s ON t.SaleId = s.Id
            WHERE s.CompanyId = @CompanyId 
              AND t.CreatedAt >= @StartDate 
              AND t.CreatedAt <= @EndDate
              AND t.IsSuccessful = 1";

        return await _dbConnection.QuerySingleAsync<decimal>(sql, new { CompanyId = companyId, StartDate = startDate, EndDate = endDate });
    }
} 
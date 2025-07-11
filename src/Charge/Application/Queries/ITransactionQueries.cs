using Charge.Application.Dtos;

namespace Charge.Application.Queries;

public interface ITransactionQueries
{
    Task<TransactionDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<TransactionDto>> GetBySaleIdAsync(Guid saleId);
    Task<IEnumerable<TransactionDto>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<TransactionDto>> GetByStatusAsync(string status);
    Task<IEnumerable<TransactionDto>> GetAllAsync();
    Task<decimal> GetTotalTransactionsAsync(Guid companyId, DateTime startDate, DateTime endDate);
} 
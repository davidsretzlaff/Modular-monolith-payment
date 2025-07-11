using Charge.Application.Dtos;

namespace Charge.Application.Services;

public interface ITransactionService
{
    Task<TransactionDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<TransactionDto>> GetBySaleIdAsync(Guid saleId);
    Task<IEnumerable<TransactionDto>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<TransactionDto>> GetByStatusAsync(string status);
    Task<IEnumerable<TransactionDto>> GetAllAsync();
    Task<TransactionDto> CreateAsync(CreateTransactionDto createDto);
    Task UpdateStatusAsync(Guid id, string status);
    Task<decimal> GetTotalTransactionsAsync(Guid companyId, DateTime startDate, DateTime endDate);
} 
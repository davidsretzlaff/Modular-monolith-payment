using Charge.Application.Dtos;
using Charge.Application.Queries;
using Charge.Domain.Entities;
using Charge.Domain.Repositories;

namespace Charge.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionQueries _transactionQueries;

    public TransactionService(ITransactionRepository transactionRepository, ITransactionQueries transactionQueries)
    {
        _transactionRepository = transactionRepository;
        _transactionQueries = transactionQueries;
    }

    public async Task<TransactionDto?> GetByIdAsync(Guid id)
    {
        return await _transactionQueries.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TransactionDto>> GetBySaleIdAsync(Guid saleId)
    {
        return await _transactionQueries.GetBySaleIdAsync(saleId);
    }

    public async Task<IEnumerable<TransactionDto>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _transactionQueries.GetByCustomerIdAsync(customerId);
    }

    public async Task<IEnumerable<TransactionDto>> GetByStatusAsync(string status)
    {
        return await _transactionQueries.GetByStatusAsync(status);
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync()
    {
        return await _transactionQueries.GetAllAsync();
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionDto createDto)
    {
        var transaction = new Transaction(
            createDto.SaleId,
            createDto.Amount,
            createDto.PaymentMethod,
            createDto.Status
        );

        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();

        return await _transactionQueries.GetByIdAsync(transaction.Id) ?? 
               throw new InvalidOperationException("Failed to retrieve created transaction");
    }

    public async Task UpdateStatusAsync(Guid id, string status)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if (transaction == null)
            throw new InvalidOperationException("Transaction not found");

        transaction.UpdateStatus(status);
        _transactionRepository.Update(transaction);
        await _transactionRepository.SaveChangesAsync();
    }

    public async Task<decimal> GetTotalTransactionsAsync(Guid companyId, DateTime startDate, DateTime endDate)
    {
        return await _transactionQueries.GetTotalTransactionsAsync(companyId, startDate, endDate);
    }
} 
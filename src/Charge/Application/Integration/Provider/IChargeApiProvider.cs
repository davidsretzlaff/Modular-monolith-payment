namespace Charge.Application.Integration.Provider;

public interface IChargeApiProvider
{
    Task<bool> SaleExistsAsync(Guid saleId);
    Task<bool> TransactionExistsAsync(Guid transactionId);
    Task<bool> IsSaleActiveAsync(Guid saleId);
    Task<bool> IsTransactionSuccessfulAsync(Guid transactionId);
    Task<SaleBasicInfo?> GetSaleBasicInfoAsync(Guid saleId);
    Task<TransactionBasicInfo?> GetTransactionBasicInfoAsync(Guid transactionId);
    Task<decimal> GetTotalRevenueAsync(Guid companyId, DateTime startDate, DateTime endDate);
}

public class SaleBasicInfo
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid CompanyId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TransactionBasicInfo
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
    public DateTime CreatedAt { get; set; }
} 
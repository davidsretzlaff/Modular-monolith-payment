namespace Charge.Application.Dtos;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid SaleId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties for complex queries
    public SaleDto? Sale { get; set; }
}

public class CreateTransactionDto
{
    public Guid SaleId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class UpdateTransactionDto
{
    public string Status { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
} 
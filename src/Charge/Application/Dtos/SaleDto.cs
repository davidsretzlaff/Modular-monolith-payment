using Shared.Core;

namespace Charge.Application.Dtos;

public class SaleDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid CompanyId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties for complex queries
    public CustomerDto? Customer { get; set; }
    public CompanyDto? Company { get; set; }
    public ICollection<TransactionDto>? Transactions { get; set; }
}

public class CreateSaleDto
{
    public Guid CustomerId { get; set; }
    public Guid CompanyId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class UpdateSaleDto
{
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
} 
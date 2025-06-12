namespace Catalog.Application.Dtos;

public class CouponDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public bool IsPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public bool IsActive { get; set; }
    public int UsageLimit { get; set; }
    public int UsedCount { get; set; }
    public Guid? PlanId { get; set; }
    public PlanDto? Plan { get; set; } // Relacionamento altamente acoplado
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
} 
namespace Catalog.Application.Dtos;

public class CreateCouponDto
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public bool IsPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public int UsageLimit { get; set; }
    public Guid? PlanId { get; set; }
} 
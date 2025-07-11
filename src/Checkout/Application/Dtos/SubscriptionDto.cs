using Shared.Core;

namespace Checkout.Application.Dtos;

public class SubscriptionDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid PlanId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties for complex queries
    public CustomerDto? Customer { get; set; }
    public PlanDto? Plan { get; set; }
}

public class UpdateSubscriptionDto
{
    public string Status { get; set; } = string.Empty;
    public DateTime? EndDate { get; set; }
} 
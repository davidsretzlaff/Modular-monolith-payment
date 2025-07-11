using Shared.Core.Cqrs;

namespace Catalog.Application.Commands.Plans;

public class CreatePlanCommand : ICommand<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid CompanyId { get; set; }
    public int DurationInDays { get; set; }
} 
using Shared.Core.Cqrs;

namespace Catalog.Application.Commands.Plans;

public class ActivatePlanCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
}

public class DeactivatePlanCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
} 
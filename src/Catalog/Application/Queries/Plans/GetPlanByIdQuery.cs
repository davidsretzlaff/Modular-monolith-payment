using Catalog.Application.Dtos;
using Shared.Core.Cqrs;

namespace Catalog.Application.Queries.Plans;

public class GetPlanByIdQuery : IQuery<PlanDto?>
{
    public Guid Id { get; set; }

    public GetPlanByIdQuery(Guid id)
    {
        Id = id;
    }
} 
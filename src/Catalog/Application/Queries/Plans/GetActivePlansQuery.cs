using Catalog.Application.Dtos;
using Shared.Core.Cqrs;

namespace Catalog.Application.Queries.Plans;

public class GetActivePlansQuery : IQuery<IEnumerable<PlanDto>>
{
}

public class GetAllPlansQuery : IQuery<IEnumerable<PlanDto>>
{
} 
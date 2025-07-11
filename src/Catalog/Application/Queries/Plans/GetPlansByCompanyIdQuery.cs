using Catalog.Application.Dtos;
using Shared.Core.Cqrs;

namespace Catalog.Application.Queries.Plans;

public class GetPlansByCompanyIdQuery : IQuery<IEnumerable<PlanDto>>
{
    public Guid CompanyId { get; set; }

    public GetPlansByCompanyIdQuery(Guid companyId)
    {
        CompanyId = companyId;
    }
} 
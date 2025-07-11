using Admin.Application.Dtos;
using Shared.Core.Cqrs;

namespace Admin.Application.Queries.Companies;

public class GetCompanyByIdQuery : IQuery<CompanyDto?>
{
    public Guid Id { get; set; }

    public GetCompanyByIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetAllCompaniesQuery : IQuery<IEnumerable<CompanyDto>>
{
}

public class GetActiveCompaniesQuery : IQuery<IEnumerable<CompanyDto>>
{
} 
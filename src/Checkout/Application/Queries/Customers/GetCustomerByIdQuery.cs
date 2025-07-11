using Checkout.Application.Dtos;
using Shared.Core.Cqrs;

namespace Checkout.Application.Queries.Customers;

public class GetCustomerByIdQuery : IQuery<CustomerDto?>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }

    public GetCustomerByIdQuery(Guid id, Guid companyId)
    {
        Id = id;
        CompanyId = companyId;
    }
}

public class GetCustomersByCompanyIdQuery : IQuery<IEnumerable<CustomerDto>>
{
    public Guid CompanyId { get; set; }

    public GetCustomersByCompanyIdQuery(Guid companyId)
    {
        CompanyId = companyId;
    }
}

public class GetActiveCustomersQuery : IQuery<IEnumerable<CustomerDto>>
{
    public Guid CompanyId { get; set; }

    public GetActiveCustomersQuery(Guid companyId)
    {
        CompanyId = companyId;
    }
} 
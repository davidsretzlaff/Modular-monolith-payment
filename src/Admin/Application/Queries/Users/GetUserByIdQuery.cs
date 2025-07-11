using Admin.Application.Dtos;
using Shared.Core.Cqrs;

namespace Admin.Application.Queries.Users;

public class GetUserByIdQuery : IQuery<UserDto?>
{
    public Guid Id { get; set; }

    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }
}

public class GetUsersByCompanyIdQuery : IQuery<IEnumerable<UserDto>>
{
    public Guid CompanyId { get; set; }

    public GetUsersByCompanyIdQuery(Guid companyId)
    {
        CompanyId = companyId;
    }
}

public class GetAllUsersQuery : IQuery<IEnumerable<UserDto>>
{
}

public class GetActiveUsersQuery : IQuery<IEnumerable<UserDto>>
{
} 
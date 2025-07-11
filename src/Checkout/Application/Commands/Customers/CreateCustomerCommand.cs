using Shared.Core.Cqrs;

namespace Checkout.Application.Commands.Customers;

public class CreateCustomerCommand : ICommand<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid CompanyId { get; set; }
} 
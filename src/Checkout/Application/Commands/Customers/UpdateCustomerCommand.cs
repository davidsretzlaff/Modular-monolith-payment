using Shared.Core.Cqrs;

namespace Checkout.Application.Commands.Customers;

public class UpdateCustomerCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid CompanyId { get; set; } // For validation
}

public class ActivateCustomerCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // For validation
}

public class DeactivateCustomerCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // For validation
} 
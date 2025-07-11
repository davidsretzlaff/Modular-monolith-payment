using Shared.Core.Cqrs;

namespace Admin.Application.Commands.Users;

public class UpdateUserCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid CompanyId { get; set; } // For validation
}

public class ActivateUserCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // For validation
}

public class DeactivateUserCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // For validation
} 
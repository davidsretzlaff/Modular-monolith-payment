using Shared.Core.Cqrs;

namespace Admin.Application.Commands.Companies;

public class UpdateCompanyCommand : ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class ActivateCompanyCommand : ICommand
{
    public Guid Id { get; set; }
}

public class DeactivateCompanyCommand : ICommand
{
    public Guid Id { get; set; }
} 
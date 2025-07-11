using Shared.Core.Cqrs;

namespace Admin.Application.Commands.Companies;

public class CreateCompanyCommand : ICommand<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
} 
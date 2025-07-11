using Admin.Domain.Entities;
using Admin.Domain.Repositories;
using Shared.Core.Cqrs;

namespace Admin.Application.Commands.Users;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly ICompanyRepository _companyRepository;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        ICompanyRepository companyRepository)
    {
        _userRepository = userRepository;
        _companyRepository = companyRepository;
    }

    public async Task<Guid> HandleAsync(CreateUserCommand command)
    {
        // Validate company exists
        var company = await _companyRepository.GetByIdAsync(command.CompanyId);
        if (company == null)
            throw new InvalidOperationException("Company not found");

        if (!company.IsActive)
            throw new InvalidOperationException("Company is inactive");

        // Create user entity
        var user = new User(command.Name, command.Email, command.CompanyId);

        // Save to repository
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user.Id;
    }
} 
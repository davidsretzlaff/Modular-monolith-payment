using Admin.Domain.Entities;
using Admin.Domain.Repositories;
using Shared.Core.Cqrs;

namespace Admin.Application.Commands.Companies;

public class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;

    public CreateCompanyCommandHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<Guid> HandleAsync(CreateCompanyCommand command)
    {
        // Create company entity
        var company = new Company(command.Name, command.Email);

        // Save to repository
        await _companyRepository.AddAsync(company);
        await _companyRepository.SaveChangesAsync();

        return company.Id;
    }
} 
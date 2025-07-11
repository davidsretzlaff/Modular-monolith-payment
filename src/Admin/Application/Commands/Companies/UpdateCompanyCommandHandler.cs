using Admin.Domain.Repositories;
using Shared.Core.Cqrs;

namespace Admin.Application.Commands.Companies;

public class UpdateCompanyCommandHandler : ICommandHandler<UpdateCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;

    public UpdateCompanyCommandHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task HandleAsync(UpdateCompanyCommand command)
    {
        var company = await _companyRepository.GetByIdAsync(command.Id);
        if (company == null)
            throw new InvalidOperationException("Company not found");

        company.Update(command.Name, command.Email);
        
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _companyRepository.SaveChangesAsync();
    }
}

public class ActivateCompanyCommandHandler : ICommandHandler<ActivateCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;

    public ActivateCompanyCommandHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task HandleAsync(ActivateCompanyCommand command)
    {
        var company = await _companyRepository.GetByIdAsync(command.Id);
        if (company == null)
            throw new InvalidOperationException("Company not found");

        company.Activate();
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _companyRepository.SaveChangesAsync();
    }
}

public class DeactivateCompanyCommandHandler : ICommandHandler<DeactivateCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;

    public DeactivateCompanyCommandHandler(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task HandleAsync(DeactivateCompanyCommand command)
    {
        var company = await _companyRepository.GetByIdAsync(command.Id);
        if (company == null)
            throw new InvalidOperationException("Company not found");

        company.Deactivate();
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _companyRepository.SaveChangesAsync();
    }
} 
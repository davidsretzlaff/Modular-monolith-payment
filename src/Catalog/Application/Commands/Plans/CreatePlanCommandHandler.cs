using Catalog.Application.Integration;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Shared.Core.Cqrs;

namespace Catalog.Application.Commands.Plans;

public class CreatePlanCommandHandler : ICommandHandler<CreatePlanCommand, Guid>
{
    private readonly IPlanRepository _planRepository;
    private readonly IAdminApiProvider _adminApiProvider;

    public CreatePlanCommandHandler(
        IPlanRepository planRepository,
        IAdminApiProvider adminApiProvider)
    {
        _planRepository = planRepository;
        _adminApiProvider = adminApiProvider;
    }

    public async Task<Guid> HandleAsync(CreatePlanCommand command)
    {
        var companyExists = await _adminApiProvider.CompanyExistsAsync(command.CompanyId);
        if (!companyExists)
            throw new InvalidOperationException("Company not found");

        var plan = new Plan(
            command.Name,
            command.Description,
            command.Price,
            command.CompanyId,
            command.DurationInDays);

        await _planRepository.AddAsync(plan);
        await _planRepository.SaveChangesAsync();

        return plan.Id;
    }
} 
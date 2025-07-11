using Catalog.Domain.Repositories;
using Shared.Core.Cqrs;

namespace Catalog.Application.Commands.Plans;

public class ActivatePlanCommandHandler : ICommandHandler<ActivatePlanCommand>
{
    private readonly IPlanRepository _planRepository;

    public ActivatePlanCommandHandler(IPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }

    public async Task HandleAsync(ActivatePlanCommand command)
    {
        var plan = await _planRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (plan == null)
            throw new InvalidOperationException("Plan not found");

        plan.Activate();
        await _planRepository.SaveChangesAsync();
    }
}

public class DeactivatePlanCommandHandler : ICommandHandler<DeactivatePlanCommand>
{
    private readonly IPlanRepository _planRepository;

    public DeactivatePlanCommandHandler(IPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }

    public async Task HandleAsync(DeactivatePlanCommand command)
    {
        var plan = await _planRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (plan == null)
            throw new InvalidOperationException("Plan not found");

        plan.Deactivate();
        await _planRepository.SaveChangesAsync();
    }
} 
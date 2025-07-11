using Catalog.Domain.Repositories;
using Shared.Core.Cqrs;

namespace Catalog.Application.Commands.Plans;

public class UpdatePlanCommandHandler : ICommandHandler<UpdatePlanCommand>
{
    private readonly IPlanRepository _planRepository;

    public UpdatePlanCommandHandler(IPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }

    public async Task HandleAsync(UpdatePlanCommand command)
    {
        var plan = await _planRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (plan == null)
            throw new InvalidOperationException("Plan not found");

        plan.Update(command.Name, command.Description, command.Price, command.DurationInDays);
        
        await _planRepository.SaveChangesAsync();
    }
} 
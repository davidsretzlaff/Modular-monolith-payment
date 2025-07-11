using Admin.Domain.Repositories;
using Shared.Core.Cqrs;

namespace Admin.Application.Commands.Users;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(UpdateUserCommand command)
    {
        var user = await _userRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        user.Update(command.Name, command.Email);
        
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _userRepository.SaveChangesAsync();
    }
}

public class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public ActivateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(ActivateUserCommand command)
    {
        var user = await _userRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        user.Activate();
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _userRepository.SaveChangesAsync();
    }
}

public class DeactivateUserCommandHandler : ICommandHandler<DeactivateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeactivateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleAsync(DeactivateUserCommand command)
    {
        var user = await _userRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        user.Deactivate();
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _userRepository.SaveChangesAsync();
    }
} 
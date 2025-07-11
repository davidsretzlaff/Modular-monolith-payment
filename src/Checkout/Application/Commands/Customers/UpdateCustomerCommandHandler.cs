using Checkout.Domain.Repositories;
using Shared.Core.Cqrs;

namespace Checkout.Application.Commands.Customers;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task HandleAsync(UpdateCustomerCommand command)
    {
        var customer = await _customerRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        customer.Update(command.Name, command.Email);
        
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _customerRepository.SaveChangesAsync();
    }
}

public class ActivateCustomerCommandHandler : ICommandHandler<ActivateCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public ActivateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task HandleAsync(ActivateCustomerCommand command)
    {
        var customer = await _customerRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        customer.Activate();
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _customerRepository.SaveChangesAsync();
    }
}

public class DeactivateCustomerCommandHandler : ICommandHandler<DeactivateCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public DeactivateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task HandleAsync(DeactivateCustomerCommand command)
    {
        var customer = await _customerRepository.GetByIdAsync(command.Id, command.CompanyId);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        customer.Deactivate();
        // No need for UpdateAsync with tracking enabled - EF will detect changes
        await _customerRepository.SaveChangesAsync();
    }
} 
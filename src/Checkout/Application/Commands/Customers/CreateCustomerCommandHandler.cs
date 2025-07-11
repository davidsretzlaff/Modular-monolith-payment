using Checkout.Domain.Entities;
using Checkout.Domain.Repositories;
using Shared.Core.Cqrs;
using Shared.Contracts;

namespace Checkout.Application.Commands.Customers;

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAdminApiProvider _adminApiProvider;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IAdminApiProvider adminApiProvider)
    {
        _customerRepository = customerRepository;
        _adminApiProvider = adminApiProvider;
    }

    public async Task<Guid> HandleAsync(CreateCustomerCommand command)
    {
        // Validate company exists and is active via Admin API
        var companyExists = await _adminApiProvider.CompanyExistsAsync(command.CompanyId);
        if (!companyExists)
            throw new InvalidOperationException("Company not found");

        var companyIsActive = await _adminApiProvider.IsCompanyActiveAsync(command.CompanyId);
        if (!companyIsActive)
            throw new InvalidOperationException("Company is inactive");

        // Create customer entity
        var customer = new Customer(command.Name, command.Email, command.CompanyId);

        // Save to repository
        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();

        return customer.Id;
    }
} 
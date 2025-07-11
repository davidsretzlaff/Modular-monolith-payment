using Checkout.Application.Dtos;
using Checkout.Application.Queries;
using Checkout.Domain.Entities;
using Checkout.Domain.Repositories;

namespace Checkout.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerQueries _customerQueries;

    public CustomerService(ICustomerRepository customerRepository, ICustomerQueries customerQueries)
    {
        _customerRepository = customerRepository;
        _customerQueries = customerQueries;
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        return await _customerQueries.GetByIdAsync(id);
    }

    public async Task<CustomerDto?> GetByEmailAsync(string email)
    {
        return await _customerQueries.GetByEmailAsync(email);
    }

    public async Task<IEnumerable<CustomerDto>> GetByCompanyIdAsync(Guid companyId)
    {
        return await _customerQueries.GetByCompanyIdAsync(companyId);
    }

    public async Task<IEnumerable<CustomerDto>> GetActiveAsync()
    {
        return await _customerQueries.GetActiveAsync();
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        return await _customerQueries.GetAllAsync();
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto createDto)
    {
        if (await _customerRepository.EmailExistsAsync(createDto.Email))
            throw new InvalidOperationException($"Customer with email '{createDto.Email}' already exists");

        var customer = new Customer(
            createDto.Name,
            createDto.Email,
            createDto.CompanyId
        );

        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();

        return await _customerQueries.GetByIdAsync(customer.Id) ?? 
               throw new InvalidOperationException("Failed to retrieve created customer");
    }

    public async Task UpdateAsync(Guid id, UpdateCustomerDto updateDto)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        customer.UpdateDetails(updateDto.Name, updateDto.Email);
        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync();
    }

    public async Task ActivateAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        customer.Activate();
        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync();
    }

    public async Task DeactivateAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
            throw new InvalidOperationException("Customer not found");

        customer.Deactivate();
        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _customerRepository.GetByIdAsync(id) != null;
    }

    public async Task<bool> IsActiveAsync(Guid id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        return customer?.IsActive ?? false;
    }
} 
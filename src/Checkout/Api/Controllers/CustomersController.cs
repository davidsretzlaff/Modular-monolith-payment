using Microsoft.AspNetCore.Mvc;
using Checkout.Application.Services;
using Checkout.Application.Dtos;

namespace Checkout.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetActive()
    {
        var customers = await _customerService.GetActiveAsync();
        return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<CustomerDto>> GetByEmail(string email)
    {
        var customer = await _customerService.GetByEmailAsync(email);
        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpGet("company/{companyId:guid}")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetByCompanyId(Guid companyId)
    {
        var customers = await _customerService.GetByCompanyIdAsync(companyId);
        return Ok(customers);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerDto createDto)
    {
        try
        {
            var customer = await _customerService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCustomerDto updateDto)
    {
        try
        {
            await _customerService.UpdateAsync(id, updateDto);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        try
        {
            await _customerService.ActivateAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _customerService.DeactivateAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}/exists")]
    public async Task<ActionResult<bool>> Exists(Guid id)
    {
        var exists = await _customerService.ExistsAsync(id);
        return Ok(exists);
    }

    [HttpGet("{id:guid}/active")]
    public async Task<ActionResult<bool>> IsActive(Guid id)
    {
        var isActive = await _customerService.IsActiveAsync(id);
        return Ok(isActive);
    }
} 
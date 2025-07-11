using Microsoft.AspNetCore.Mvc;
using Admin.Application.Commands.Companies;
using Admin.Application.Queries.Companies;
using Shared.Core.Cqrs;

namespace Admin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public CompaniesController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var company = await _queryDispatcher.DispatchAsync(new GetCompanyByIdQuery(id));
        if (company == null)
            return NotFound();

        return Ok(company);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var companies = await _queryDispatcher.DispatchAsync(new GetAllCompaniesQuery());
        return Ok(companies);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var companies = await _queryDispatcher.DispatchAsync(new GetActiveCompaniesQuery());
        return Ok(companies);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
    {
        try
        {
            var companyId = await _commandDispatcher.DispatchAsync(command);
            return CreatedAtAction(nameof(GetById), new { id = companyId }, new { id = companyId });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCompanyCommand command)
    {
        try
        {
            command.Id = id;
            await _commandDispatcher.DispatchAsync(command);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        try
        {
            await _commandDispatcher.DispatchAsync(new ActivateCompanyCommand { Id = id });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _commandDispatcher.DispatchAsync(new DeactivateCompanyCommand { Id = id });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 
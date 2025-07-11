using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Commands.Plans;
using Catalog.Application.Queries.Plans;
using Shared.Core.Cqrs;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlansController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public PlansController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var plan = await _queryDispatcher.DispatchAsync(new GetPlanByIdQuery(id));
        if (plan == null)
            return NotFound();

        return Ok(plan);
    }

    [HttpGet("company/{companyId}")]
    public async Task<IActionResult> GetByCompanyId(Guid companyId)
    {
        var plans = await _queryDispatcher.DispatchAsync(new GetPlansByCompanyIdQuery(companyId));
        return Ok(plans);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var plans = await _queryDispatcher.DispatchAsync(new GetActivePlansQuery());
        return Ok(plans);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var plans = await _queryDispatcher.DispatchAsync(new GetAllPlansQuery());
        return Ok(plans);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlanCommand command)
    {
        try
        {
            var planId = await _commandDispatcher.DispatchAsync(command);
            return CreatedAtAction(nameof(GetById), new { id = planId }, new { id = planId });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePlanCommand command)
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
    public async Task<IActionResult> Activate(Guid id, [FromQuery] Guid companyId)
    {
        try
        {
            await _commandDispatcher.DispatchAsync(new ActivatePlanCommand { Id = id, CompanyId = companyId });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id, [FromQuery] Guid companyId)
    {
        try
        {
            await _commandDispatcher.DispatchAsync(new DeactivatePlanCommand { Id = id, CompanyId = companyId });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 
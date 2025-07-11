using Microsoft.AspNetCore.Mvc;
using Admin.Application.Commands.Users;
using Admin.Application.Queries.Users;
using Shared.Core.Cqrs;

namespace Admin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public UsersController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _queryDispatcher.DispatchAsync(new GetUserByIdQuery(id));
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _queryDispatcher.DispatchAsync(new GetAllUsersQuery());
        return Ok(users);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var users = await _queryDispatcher.DispatchAsync(new GetActiveUsersQuery());
        return Ok(users);
    }

    [HttpGet("company/{companyId}")]
    public async Task<IActionResult> GetByCompanyId(Guid companyId)
    {
        var users = await _queryDispatcher.DispatchAsync(new GetUsersByCompanyIdQuery(companyId));
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        try
        {
            var userId = await _commandDispatcher.DispatchAsync(command);
            return CreatedAtAction(nameof(GetById), new { id = userId }, new { id = userId });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command, [FromQuery] Guid companyId)
    {
        try
        {
            command.Id = id;
            command.CompanyId = companyId;
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
            await _commandDispatcher.DispatchAsync(new ActivateUserCommand { Id = id, CompanyId = companyId });
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
            await _commandDispatcher.DispatchAsync(new DeactivateUserCommand { Id = id, CompanyId = companyId });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 
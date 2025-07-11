using Microsoft.AspNetCore.Mvc;
using Charge.Application.Services;
using Charge.Application.Dtos;

namespace Charge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetAll()
    {
        var sales = await _saleService.GetAllAsync();
        return Ok(sales);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetActive()
    {
        var sales = await _saleService.GetActiveAsync();
        return Ok(sales);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SaleDto>> GetById(Guid id)
    {
        var sale = await _saleService.GetByIdAsync(id);
        if (sale == null)
            return NotFound();

        return Ok(sale);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetByCustomerId(Guid customerId)
    {
        var sales = await _saleService.GetByCustomerIdAsync(customerId);
        return Ok(sales);
    }

    [HttpGet("company/{companyId:guid}")]
    public async Task<ActionResult<IEnumerable<SaleDto>>> GetByCompanyId(Guid companyId)
    {
        var sales = await _saleService.GetByCompanyIdAsync(companyId);
        return Ok(sales);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> Create(CreateSaleDto createDto)
    {
        try
        {
            var sale = await _saleService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] string status)
    {
        try
        {
            await _saleService.UpdateStatusAsync(id, status);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        try
        {
            await _saleService.CancelAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("revenue/{companyId:guid}")]
    public async Task<ActionResult<decimal>> GetTotalRevenue(Guid companyId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var revenue = await _saleService.GetTotalRevenueAsync(companyId, startDate, endDate);
        return Ok(revenue);
    }
} 
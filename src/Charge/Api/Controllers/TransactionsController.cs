using Microsoft.AspNetCore.Mvc;
using Charge.Application.Services;
using Charge.Application.Dtos;

namespace Charge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll()
    {
        var transactions = await _transactionService.GetAllAsync();
        return Ok(transactions);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionDto>> GetById(Guid id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);
        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpGet("sale/{saleId:guid}")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetBySaleId(Guid saleId)
    {
        var transactions = await _transactionService.GetBySaleIdAsync(saleId);
        return Ok(transactions);
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetByCustomerId(Guid customerId)
    {
        var transactions = await _transactionService.GetByCustomerIdAsync(customerId);
        return Ok(transactions);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetByStatus(string status)
    {
        var transactions = await _transactionService.GetByStatusAsync(status);
        return Ok(transactions);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> Create(CreateTransactionDto createDto)
    {
        try
        {
            var transaction = await _transactionService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
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
            await _transactionService.UpdateStatusAsync(id, status);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("total/{companyId:guid}")]
    public async Task<ActionResult<decimal>> GetTotalTransactions(Guid companyId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var total = await _transactionService.GetTotalTransactionsAsync(companyId, startDate, endDate);
        return Ok(total);
    }
} 
using Charge.Application.Dtos;
using Charge.Application.Queries;
using Charge.Domain.Entities;
using Charge.Domain.Repositories;

namespace Charge.Application.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleQueries _saleQueries;

    public SaleService(ISaleRepository saleRepository, ISaleQueries saleQueries)
    {
        _saleRepository = saleRepository;
        _saleQueries = saleQueries;
    }

    public async Task<SaleDto?> GetByIdAsync(Guid id)
    {
        return await _saleQueries.GetByIdAsync(id);
    }

    public async Task<IEnumerable<SaleDto>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _saleQueries.GetByCustomerIdAsync(customerId);
    }

    public async Task<IEnumerable<SaleDto>> GetByCompanyIdAsync(Guid companyId)
    {
        return await _saleQueries.GetByCompanyIdAsync(companyId);
    }

    public async Task<IEnumerable<SaleDto>> GetActiveAsync()
    {
        return await _saleQueries.GetActiveAsync();
    }

    public async Task<IEnumerable<SaleDto>> GetAllAsync()
    {
        return await _saleQueries.GetAllAsync();
    }

    public async Task<SaleDto> CreateAsync(CreateSaleDto createDto)
    {
        var sale = new Sale(
            createDto.CustomerId,
            createDto.CompanyId,
            createDto.TotalAmount,
            createDto.Description
        );

        await _saleRepository.AddAsync(sale);
        await _saleRepository.SaveChangesAsync();

        return await _saleQueries.GetByIdAsync(sale.Id) ?? 
               throw new InvalidOperationException("Failed to retrieve created sale");
    }

    public async Task UpdateStatusAsync(Guid id, string status)
    {
        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale == null)
            throw new InvalidOperationException("Sale not found");

        sale.UpdateStatus(status);
        _saleRepository.Update(sale);
        await _saleRepository.SaveChangesAsync();
    }

    public async Task CancelAsync(Guid id)
    {
        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale == null)
            throw new InvalidOperationException("Sale not found");

        sale.Cancel();
        _saleRepository.Update(sale);
        await _saleRepository.SaveChangesAsync();
    }

    public async Task<decimal> GetTotalRevenueAsync(Guid companyId, DateTime startDate, DateTime endDate)
    {
        return await _saleQueries.GetTotalRevenueAsync(companyId, startDate, endDate);
    }
} 
using Charge.Application.Dtos;

namespace Charge.Application.Services;

public interface ISaleService
{
    Task<SaleDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<SaleDto>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<SaleDto>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<SaleDto>> GetActiveAsync();
    Task<IEnumerable<SaleDto>> GetAllAsync();
    Task<SaleDto> CreateAsync(CreateSaleDto createDto);
    Task UpdateStatusAsync(Guid id, string status);
    Task CancelAsync(Guid id);
    Task<decimal> GetTotalRevenueAsync(Guid companyId, DateTime startDate, DateTime endDate);
} 
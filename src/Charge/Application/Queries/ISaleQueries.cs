using Charge.Application.Dtos;

namespace Charge.Application.Queries;

public interface ISaleQueries
{
    Task<SaleDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<SaleDto>> GetByCustomerIdAsync(Guid customerId);
    Task<IEnumerable<SaleDto>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<SaleDto>> GetActiveAsync();
    Task<IEnumerable<SaleDto>> GetAllAsync();
    Task<decimal> GetTotalRevenueAsync(Guid companyId, DateTime startDate, DateTime endDate);
} 
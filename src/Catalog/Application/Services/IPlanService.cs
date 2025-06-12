using Catalog.Application.Dtos;

namespace Catalog.Application.Services;

public interface IPlanService
{
    Task<PlanDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<PlanDto>> GetByCompanyIdAsync(Guid companyId);
    Task<IEnumerable<PlanDto>> GetActiveAsync();
    Task<IEnumerable<PlanDto>> GetAllAsync();
    Task<PlanDto> CreateAsync(CreatePlanDto createDto);
    Task UpdatePriceAsync(Guid id, decimal newPrice);
    Task UpdateDetailsAsync(Guid id, string name, string description);
    Task ActivateAsync(Guid id);
    Task DeactivateAsync(Guid id);
} 
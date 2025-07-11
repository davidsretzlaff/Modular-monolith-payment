using Catalog.Application.Dtos;
using Catalog.Application.Queries;
using Catalog.Application.Integration;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;

namespace Catalog.Application.Services;

public class PlanService : IPlanService
{
    private readonly IPlanRepository _planRepository;
    private readonly IPlanQueries _planQueries;
    private readonly IAdminApiProvider _adminApiProvider;

    public PlanService(
        IPlanRepository planRepository, 
        IPlanQueries planQueries,
        IAdminApiProvider adminApiProvider)
    {
        _planRepository = planRepository;
        _planQueries = planQueries;
        _adminApiProvider = adminApiProvider;
    }

    public async Task<PlanDto?> GetByIdAsync(Guid id)
    {
        return await _planQueries.GetByIdAsync(id);
    }

    public async Task<IEnumerable<PlanDto>> GetByCompanyIdAsync(Guid companyId)
    {
        return await _planQueries.GetByCompanyIdAsync(companyId);
    }

    public async Task<IEnumerable<PlanDto>> GetActiveAsync()
    {
        return await _planQueries.GetActiveAsync();
    }

    public async Task<IEnumerable<PlanDto>> GetAllAsync()
    {
        return await _planQueries.GetAllAsync();
    }

    public async Task<PlanDto> CreateAsync(CreatePlanDto createDto)
    {
        // Validate company exists using Admin integration
        if (!await _adminApiProvider.CompanyExistsAsync(createDto.CompanyId))
            throw new InvalidOperationException($"Company with ID '{createDto.CompanyId}' does not exist");

        var plan = new Plan(
            createDto.Name,
            createDto.Description,
            createDto.CompanyId,
            createDto.DurationInDays
        );

        // Add pricing option
        plan.AddPricingOption(createDto.Price, 1); // Default to 1 month billing cycle

        await _planRepository.AddAsync(plan);
        await _planRepository.SaveChangesAsync();

        // Return the created plan using Dapper query with Company data
        return await _planQueries.GetByIdAsync(plan.Id) ?? 
               throw new InvalidOperationException("Failed to retrieve created plan");
    }

    public async Task UpdatePriceAsync(Guid id, decimal newPrice)
    {
        var plan = await _planRepository.GetByIdAsync(id);
        if (plan == null)
            throw new InvalidOperationException("Plan not found");

        // Get the first pricing option or create a new one
        var pricingOption = plan.PricingOptions.FirstOrDefault();
        if (pricingOption == null)
        {
            plan.AddPricingOption(newPrice, 1);
        }
        else
        {
            pricingOption.UpdatePrice(newPrice);
        }

        _planRepository.Update(plan);
        await _planRepository.SaveChangesAsync();
    }

    public async Task UpdateDetailsAsync(Guid id, string name, string description)
    {
        var plan = await _planRepository.GetByIdAsync(id);
        if (plan == null)
            throw new InvalidOperationException("Plan not found");

        plan.UpdateDetails(name, description);
        _planRepository.Update(plan);
        await _planRepository.SaveChangesAsync();
    }

    public async Task ActivateAsync(Guid id)
    {
        var plan = await _planRepository.GetByIdAsync(id);
        if (plan == null)
            throw new InvalidOperationException("Plan not found");

        plan.Activate();
        _planRepository.Update(plan);
        await _planRepository.SaveChangesAsync();
    }

    public async Task DeactivateAsync(Guid id)
    {
        var plan = await _planRepository.GetByIdAsync(id);
        if (plan == null)
            throw new InvalidOperationException("Plan not found");

        plan.Deactivate();
        _planRepository.Update(plan);
        await _planRepository.SaveChangesAsync();
    }
} 
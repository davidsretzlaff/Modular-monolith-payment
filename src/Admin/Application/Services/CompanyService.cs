using Admin.Application.Dtos;
using Admin.Application.Queries;
using Admin.Domain.Entities;
using Admin.Domain.Repositories;

namespace Admin.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyQueries _companyQueries;

    public CompanyService(ICompanyRepository companyRepository, ICompanyQueries companyQueries)
    {
        _companyRepository = companyRepository;
        _companyQueries = companyQueries;
    }

    public async Task<CompanyDto?> GetByIdAsync(Guid id)
    {
        return await _companyQueries.GetByIdAsync(id);
    }

    public async Task<IEnumerable<CompanyDto>> GetActiveAsync()
    {
        return await _companyQueries.GetActiveAsync();
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        return await _companyQueries.GetAllAsync();
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyDto createDto)
    {
        var company = new Company(
            createDto.Name,
            createDto.Email,
            createDto.Phone
        );

        await _companyRepository.AddAsync(company);
        await _companyRepository.SaveChangesAsync();

        return await _companyQueries.GetByIdAsync(company.Id) ?? 
               throw new InvalidOperationException("Failed to retrieve created company");
    }

    public async Task UpdateAsync(Guid id, UpdateCompanyDto updateDto)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null)
            throw new InvalidOperationException("Company not found");

        company.UpdateDetails(updateDto.Name, updateDto.Email, updateDto.Phone);
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
    }

    public async Task ActivateAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null)
            throw new InvalidOperationException("Company not found");

        company.Activate();
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
    }

    public async Task DeactivateAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if (company == null)
            throw new InvalidOperationException("Company not found");

        company.Deactivate();
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _companyRepository.GetByIdAsync(id) != null;
    }
} 
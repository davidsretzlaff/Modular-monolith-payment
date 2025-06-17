using Modular.Checkout.Application.Dtos;

namespace Modular.Checkout.Application.Services;

public interface ISubscriptionService
{
    Task<Guid> CreateAsync(CreateSubscriptionDto dto, Guid companyId);
    Task CancelAsync(Guid id, Guid companyId);
    Task ReactivateAsync(Guid id, Guid companyId);
} 
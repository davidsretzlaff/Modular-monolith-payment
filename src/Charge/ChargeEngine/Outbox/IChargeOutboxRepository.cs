using Modular.Charge.ChargeEngine.Outbox;

namespace Modular.Charge.ChargeEngine.Outbox;

public interface IChargeOutboxRepository
{
    Task AddAsync(ChargeOutbox outbox);
    Task<IEnumerable<ChargeOutbox>> GetPendingMessagesAsync();
    Task MarkAsProcessedAsync(Guid id);
    Task IncrementRetryCountAsync(Guid id);
    Task SaveChangesAsync();
} 
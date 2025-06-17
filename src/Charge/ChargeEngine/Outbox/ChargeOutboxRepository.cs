using Microsoft.EntityFrameworkCore;
using Modular.Charge.Infrastructure.Data;

namespace Modular.Charge.ChargeEngine.Outbox;

public class ChargeOutboxRepository : IChargeOutboxRepository
{
    private readonly ChargeDbContext _context;
    private const int MaxRetries = 3;

    public ChargeOutboxRepository(ChargeDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChargeOutbox outbox)
    {
        await _context.ChargeOutbox.AddAsync(outbox);
    }

    public async Task<IEnumerable<ChargeOutbox>> GetPendingMessagesAsync()
    {
        return await _context.ChargeOutbox
            .Where(o => o.ProcessedAt == null && o.RetryCount < MaxRetries)
            .OrderBy(o => o.CreatedAt)
            .Take(100)
            .ToListAsync();
    }

    public async Task MarkAsProcessedAsync(Guid id)
    {
        var outbox = await _context.ChargeOutbox.FindAsync(id);
        if (outbox != null)
        {
            outbox.MarkAsProcessed();
        }
    }

    public async Task IncrementRetryCountAsync(Guid id)
    {
        var outbox = await _context.ChargeOutbox.FindAsync(id);
        if (outbox != null)
        {
            outbox.IncrementRetryCount();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
} 
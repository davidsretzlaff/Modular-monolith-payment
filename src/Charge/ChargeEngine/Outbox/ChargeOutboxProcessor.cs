using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Modular.Charge.ChargeEngine.Messaging;
using Modular.Charge.Domain.Repositories;
using System.Text.Json;

namespace Modular.Charge.ChargeEngine.Outbox;

public class ChargeOutboxProcessor : BackgroundService
{
    private readonly ILogger<ChargeOutboxProcessor> _logger;
    private readonly IChargeOutboxRepository _outboxRepository;
    private readonly IChargeMessageProducer _messageProducer;
    private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(5);

    public ChargeOutboxProcessor(
        ILogger<ChargeOutboxProcessor> logger,
        IChargeOutboxRepository outboxRepository,
        IChargeMessageProducer messageProducer)
    {
        _logger = logger;
        _outboxRepository = outboxRepository;
        _messageProducer = messageProducer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var messages = await _outboxRepository.GetPendingMessagesAsync();
                foreach (var message in messages)
                {
                    try
                    {
                        await ProcessMessageAsync(message);
                        await _outboxRepository.MarkAsProcessedAsync(message.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing outbox message {MessageId}", message.Id);
                        await _outboxRepository.IncrementRetryCountAsync(message.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in outbox processor");
            }

            await Task.Delay(_pollingInterval, stoppingToken);
        }
    }

    private async Task ProcessMessageAsync(ChargeOutbox message)
    {
        switch (message.Type)
        {
            case "ChargeRequest":
                var request = JsonSerializer.Deserialize<ChargeRequest>(message.Payload);
                if (request != null)
                {
                    await _messageProducer.PublishChargeRequestAsync(request);
                }
                break;

            case "ChargeResult":
                var result = JsonSerializer.Deserialize<ChargeResult>(message.Payload);
                if (result != null)
                {
                    await _messageProducer.PublishChargeResultAsync(result);
                }
                break;

            default:
                throw new InvalidOperationException($"Unknown message type: {message.Type}");
        }
    }
} 
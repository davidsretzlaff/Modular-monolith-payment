using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Modular.Charge.ChargeEngine.Messaging.Models;
using System.Text.Json;

namespace Modular.Charge.ChargeEngine.Messaging;

public class KafkaChargeMessageProducer : IChargeMessageProducer
{
    private readonly ILogger<KafkaChargeMessageProducer> _logger;
    private readonly IProducer<string, string> _producer;
    private readonly string _producerId;

    public KafkaChargeMessageProducer(
        ILogger<KafkaChargeMessageProducer> logger,
        ProducerConfig config,
        string producerId)
    {
        _logger = logger;
        _producerId = producerId;

        config.ClientId = producerId;
        config.Acks = Acks.All;
        config.EnableIdempotence = true;
        config.MaxInFlight = 1;

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishChargeRequestAsync(ChargeRequest request)
    {
        try
        {
            var message = new Message<string, string>
            {
                Key = request.Id.ToString(),
                Value = JsonSerializer.Serialize(request)
            };

            var result = await _producer.ProduceAsync("charge-requests", message);

            _logger.LogInformation(
                "Published charge request {RequestId} to partition {Partition}",
                request.Id,
                result.Partition);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing charge request {RequestId}", request.Id);
            throw;
        }
    }

    public async Task PublishChargeResultAsync(ChargeResult result)
    {
        try
        {
            var message = new Message<string, string>
            {
                Key = result.Id.ToString(),
                Value = JsonSerializer.Serialize(result)
            };

            var deliveryResult = await _producer.ProduceAsync("charge-results", message);

            _logger.LogInformation(
                "Published charge result {ResultId} to partition {Partition}",
                result.Id,
                deliveryResult.Partition);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing charge result {ResultId}", result.Id);
            throw;
        }
    }
} 
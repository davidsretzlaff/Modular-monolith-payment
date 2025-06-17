using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Modular.Charge.ChargeEngine.Messaging.Models;
using Modular.Charge.ChargeEngine.Pipeline;
using System.Text.Json;

namespace Modular.Charge.ChargeEngine.Messaging;

public class KafkaChargeMessageConsumer : IChargeMessageConsumer, IDisposable
{
    private readonly ILogger<KafkaChargeMessageConsumer> _logger;
    private readonly IConsumer<string, string> _consumer;
    private readonly string _consumerGroup;
    private readonly string _consumerId;
    private readonly IChargeProcessingPipeline _pipeline;
    private readonly IChargeMessageProducer _producer;

    public KafkaChargeMessageConsumer(
        ILogger<KafkaChargeMessageConsumer> logger,
        ConsumerConfig config,
        string consumerGroup,
        string consumerId,
        IChargeProcessingPipeline pipeline,
        IChargeMessageProducer producer)
    {
        _logger = logger;
        _consumerGroup = consumerGroup;
        _consumerId = consumerId;
        _pipeline = pipeline;
        _producer = producer;

        config.GroupId = consumerGroup;
        config.ClientId = consumerId;
        config.AutoOffsetReset = AutoOffsetReset.Earliest;
        config.EnableAutoCommit = false;

        _consumer = new ConsumerBuilder<string, string>(config).Build();
    }

    public async Task ConsumeChargeRequestsAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe("charge-requests");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var result = _consumer.Consume(cancellationToken);
                if (result == null) continue;

                var request = JsonSerializer.Deserialize<ChargeRequest>(result.Message.Value);
                if (request == null)
                {
                    _logger.LogError("Failed to deserialize charge request");
                    continue;
                }

                _logger.LogInformation(
                    "Processing charge request {RequestId} from partition {Partition}",
                    request.Id,
                    result.Partition);

                var context = new ChargeContext(request);
                var stepResult = await _pipeline.ExecuteAsync(context);

                if (stepResult.ShouldRetry)
                {
                    request.RetryCount++;
                    request.ScheduledFor = DateTime.UtcNow.Add(stepResult.RetryAfter.Value);
                    await _producer.PublishChargeRequestAsync(request);
                }

                // Commit manual após processamento
                _consumer.Commit(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
            }
        }
    }

    public async Task HandleChargeResultAsync(ChargeResult result)
    {
        // Implementar lógica de tratamento do resultado
        // Por exemplo, atualizar status da venda, enviar notificações, etc.
        _logger.LogInformation("Handling charge result {ResultId}", result.Id);
    }

    public void Dispose()
    {
        _consumer?.Dispose();
    }
} 
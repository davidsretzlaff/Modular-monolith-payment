using Modular.Charge.ChargeEngine.Messaging.Models;

namespace Modular.Charge.ChargeEngine.Messaging;

public interface IChargeMessageProducer
{
    Task PublishChargeRequestAsync(ChargeRequest request);
    Task PublishChargeResultAsync(ChargeResult result);
} 
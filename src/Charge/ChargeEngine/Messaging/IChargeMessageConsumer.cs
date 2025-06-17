using Modular.Charge.ChargeEngine.Messaging.Models;

namespace Modular.Charge.ChargeEngine.Messaging;

public interface IChargeMessageConsumer
{
    Task ConsumeChargeRequestsAsync(CancellationToken cancellationToken);
    Task HandleChargeResultAsync(ChargeResult result);
} 
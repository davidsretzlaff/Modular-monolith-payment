using Modular.Checkout.Domain.Repositories;

namespace Modular.Checkout.Infrastructure.Repositories;

public class FakePaymentGateway : IPaymentGateway
{
    private readonly Dictionary<string, bool> _payments = new();

    public Task<string> CreatePaymentAsync(decimal amount, string currency, string description)
    {
        var paymentId = Guid.NewGuid().ToString();
        _payments[paymentId] = false;
        return Task.FromResult(paymentId);
    }

    public Task<bool> ConfirmPaymentAsync(string paymentId)
    {
        if (!_payments.ContainsKey(paymentId))
            return Task.FromResult(false);

        _payments[paymentId] = true;
        return Task.FromResult(true);
    }

    public Task<bool> CancelPaymentAsync(string paymentId)
    {
        if (!_payments.ContainsKey(paymentId))
            return Task.FromResult(false);

        _payments.Remove(paymentId);
        return Task.FromResult(true);
    }
} 
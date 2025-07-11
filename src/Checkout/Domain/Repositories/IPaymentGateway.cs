namespace Checkout.Domain.Repositories;

public interface IPaymentGateway
{
    Task<string> CreatePaymentAsync(decimal amount, string currency, string description);
    Task<bool> ConfirmPaymentAsync(string paymentId);
    Task<bool> CancelPaymentAsync(string paymentId);
} 
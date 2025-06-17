using Microsoft.Extensions.Logging;
using Modular.Charge.Domain.Repositories;
using Modular.Charge.Domain.Services;

namespace Modular.Charge.ChargeEngine.Pipeline.Steps;

public class PaymentConfirmationStep : IChargeProcessingStep
{
    private readonly ILogger<PaymentConfirmationStep> _logger;
    private readonly IPaymentGateway _paymentGateway;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ISaleRepository _saleRepository;

    public int Order => 3;

    public PaymentConfirmationStep(
        ILogger<PaymentConfirmationStep> logger,
        IPaymentGateway paymentGateway,
        ITransactionRepository transactionRepository,
        ISaleRepository saleRepository)
    {
        _logger = logger;
        _paymentGateway = paymentGateway;
        _transactionRepository = transactionRepository;
        _saleRepository = saleRepository;
    }

    public async Task<StepResult> ExecuteAsync(ChargeContext context)
    {
        try
        {
            if (context.Transaction == null || context.Sale == null)
            {
                return StepResult.Failure("Transaction or Sale not found in context");
            }

            var payment = await _paymentGateway.ConfirmPaymentAsync(context.PaymentId!);
            if (!payment.Success)
            {
                context.Transaction.Status = "FAILED";
                context.Transaction.ErrorMessage = payment.ErrorMessage;
                await _transactionRepository.UpdateAsync(context.Transaction);

                context.Sale.Status = "FAILED";
                await _saleRepository.UpdateAsync(context.Sale);

                return StepResult.Failure($"Payment confirmation failed: {payment.ErrorMessage}");
            }

            context.Transaction.Status = "COMPLETED";
            context.Transaction.CompletedAt = DateTime.UtcNow;
            await _transactionRepository.UpdateAsync(context.Transaction);

            context.Sale.Status = "COMPLETED";
            context.Sale.PaidAt = DateTime.UtcNow;
            await _saleRepository.UpdateAsync(context.Sale);

            return StepResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming payment for transaction {TransactionId}", context.Transaction?.Id);
            return StepResult.Retry($"Error confirming payment: {ex.Message}", TimeSpan.FromMinutes(5));
        }
    }
} 
using Charge.ChargeEngine.Pipeline;
using Charge.Domain.Repositories;
using Charge.Domain.Services;
using Charge.ChargeEngine.Messaging.Models;

namespace Charge.ChargeEngine.Pipeline.Steps;

public class PaymentConfirmationStep : IChargeProcessingStep
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ISaleRepository _saleRepository;

    public int Order => 3;

    public PaymentConfirmationStep(
        IPaymentGateway paymentGateway,
        ITransactionRepository transactionRepository,
        ISaleRepository saleRepository)
    {
        _paymentGateway = paymentGateway;
        _transactionRepository = transactionRepository;
        _saleRepository = saleRepository;
    }

    public async Task<StepResult> ExecuteAsync(ChargeContext context)
    {
        try
        {
            // Confirm payment with gateway
            var confirmation = await _paymentGateway.ConfirmPaymentAsync(context.PaymentId!);
            
            if (confirmation.IsConfirmed)
            {
                // Update transaction status
                if (context.Transaction != null)
                {
                    context.Transaction.Confirm();
                    await _transactionRepository.UpdateAsync(context.Transaction);
                }

                // Update sale status
                if (context.Sale != null)
                {
                    context.Sale.Confirm();
                    await _saleRepository.UpdateAsync(context.Sale);
                }

                return StepResult.Success();
            }
            else
            {
                context.AddError($"Payment confirmation failed: {confirmation.ErrorMessage}");
                return StepResult.Failure(confirmation.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            context.AddError($"Payment confirmation error: {ex.Message}");
            return StepResult.Failure(ex.Message);
        }
    }
} 
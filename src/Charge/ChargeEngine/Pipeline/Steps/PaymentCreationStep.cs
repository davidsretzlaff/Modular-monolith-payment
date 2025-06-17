using Microsoft.Extensions.Logging;
using Modular.Charge.Domain.Entities;
using Modular.Charge.Domain.Repositories;
using Modular.Charge.Domain.Services;

namespace Modular.Charge.ChargeEngine.Pipeline.Steps;

public class PaymentCreationStep : IChargeProcessingStep
{
    private readonly ILogger<PaymentCreationStep> _logger;
    private readonly IPaymentGateway _paymentGateway;
    private readonly ITransactionRepository _transactionRepository;

    public int Order => 2;

    public PaymentCreationStep(
        ILogger<PaymentCreationStep> logger,
        IPaymentGateway paymentGateway,
        ITransactionRepository transactionRepository)
    {
        _logger = logger;
        _paymentGateway = paymentGateway;
        _transactionRepository = transactionRepository;
    }

    public async Task<StepResult> ExecuteAsync(ChargeContext context)
    {
        try
        {
            if (context.Sale == null)
            {
                return StepResult.Failure("Sale not found in context");
            }

            var payment = await _paymentGateway.CreatePaymentAsync(new PaymentRequest
            {
                Amount = context.Sale.Amount,
                Currency = context.Request.Currency,
                PaymentMethod = context.Request.PaymentMethod,
                Metadata = context.Request.PaymentMetadata
            });

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                SaleId = context.Sale.Id,
                Amount = context.Sale.Amount,
                Status = "PENDING",
                PaymentId = payment.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);

            context.Transaction = transaction;
            context.PaymentId = payment.Id;

            return StepResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment for sale {SaleId}", context.Sale?.Id);
            return StepResult.Retry($"Error creating payment: {ex.Message}", TimeSpan.FromMinutes(5));
        }
    }
} 
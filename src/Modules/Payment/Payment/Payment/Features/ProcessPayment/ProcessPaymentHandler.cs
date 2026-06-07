using Microsoft.EntityFrameworkCore;
using Payment.Data;
using Payment.Models;
using Payment.Payment.Features.ProcessPayment;
using Shared.Contracts.CQRS;
using MassTransit;
using Shared.Messaging.Events;
namespace Payment.Payments.Features.ProcessPayment;

internal class ProcessPaymentHandler(PaymentDbContext dbContext, IPublishEndpoint publishEndpoint)
    : ICommandHandler<ProcessPaymentCommand, ProcessPaymentResult>
{
    public async Task<ProcessPaymentResult> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken)
    {
        // 1. Idempotency Check
        var existingPayment = await dbContext.PaymentTransactions
            .AsNoTracking()
            .Where(p => p.OrderId == command.OrderId && p.Status == PaymentStatus.Completed)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingPayment != null)
        {
            // Return success but do not perform the deduction anymore (with a mild warning message)
            return new ProcessPaymentResult(
                existingPayment.Id,
                true,
                "Transaction is processed successfully. Avoid dedup payment.");
        }

        // 2. Create a new payment transaction
        var payment = PaymentTransaction.Create(command.OrderId, command.Amount, command.PaymentMethod);
        dbContext.PaymentTransactions.Add(payment);


        // 3. Simulate calling external payment gateway (Stripe, VNPay, Momo, etc.)
        // Actual, we will use HttpClient to call external payment gateway (Stripe, VNPay, Momo, etc.) here.
        bool isGatewaySuccess = true; // Simulate successful payment gateway response
        string mockGatewayTxId = $"TXN_{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";

        // 4. UPDATE PAYMENT STATUS BASED ON THE RESULT FROM PAYMENT GATEWAY
        if (isGatewaySuccess)
        {
            payment.MarkAsCompleted(mockGatewayTxId);
        }
        else
        {
            payment.MarkAsFailed(mockGatewayTxId);
        }

        // 5. Save the final result to database
        await dbContext.SaveChangesAsync(cancellationToken);

        if (payment.Status == PaymentStatus.Completed)
        {
            var eventMessage = new PaymentCompletedEvent
            {
                OrderId = payment.OrderId,
                PaymentId = payment.Id,
                Amount = payment.Amount
            };

            // Push to RabbitMQ
            await publishEndpoint.Publish(eventMessage, cancellationToken);
        }

        return new ProcessPaymentResult(
            payment.Id,
            payment.Status == PaymentStatus.Completed,
            payment.Status == PaymentStatus.Completed ? "Payment successfully." : "Payment failure."
        );
    }
}
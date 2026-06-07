using Shared.Contracts.CQRS;

namespace Payment.Payment.Features.ProcessPayment;

// Record giúp đối tượng này mang tính chất "Bất biến" (Immutable)
public record ProcessPaymentCommand(Guid OrderId, decimal Amount, string PaymentMethod)
    : ICommand<ProcessPaymentResult>;

public record ProcessPaymentResult(Guid PaymentId, bool IsSuccess, string Message);
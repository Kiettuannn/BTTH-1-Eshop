using FluentValidation;
using Payment.Payment.Features.ProcessPayment;

namespace Payment.Payments.Features.ProcessPayment;

public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
{
    public ProcessPaymentCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order id is not allowed empty.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("They payment amounts must have larger 0.");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is not allowed empty.");
    }
}
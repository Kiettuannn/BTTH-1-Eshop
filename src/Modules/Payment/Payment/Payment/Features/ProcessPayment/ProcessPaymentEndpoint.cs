using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Payment.Payment.Features.ProcessPayment;

namespace Payment.Payments.Features.ProcessPayment;

public record ProcessPaymentRequest(Guid OrderId, decimal Amount, string PaymentMethod);
public record ProcessPaymentResponse(Guid PaymentId, bool IsSuccess, string Message);

public class ProcessPaymentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/payments", async (ProcessPaymentRequest request, ISender sender) =>
        {
            
            var command = request.Adapt<ProcessPaymentCommand>();

            var result = await sender.Send(command);

            var response = result.Adapt<ProcessPaymentResponse>();

            if (!response.IsSuccess)
            {
                return Results.BadRequest(response);
            }

            return Results.Ok(response);
        })
        .WithName("ProcessPayment")
        .Produces<ProcessPaymentResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Process Payment")
        .WithDescription("Process Payment");
    }
}
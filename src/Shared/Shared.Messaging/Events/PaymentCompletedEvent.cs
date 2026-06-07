using Shared.Messaging.Events;

namespace Shared.Messaging.Events;

public record PaymentCompletedEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }
    public Guid PaymentId { get; init; }
    public decimal Amount { get; init; }
}
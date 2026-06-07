using MassTransit;
using Microsoft.EntityFrameworkCore;
using Ordering.Data;
using Shared.Messaging.Events;

namespace Ordering.Orders.EventHandlers;

public class PaymentCompletedEventConsumer(OrderingDbContext dbContext)
    : IConsumer<PaymentCompletedEvent>
{
    public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        var message = context.Message;

        // 1. Find the order in Ordering DB
        var order = await dbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == message.OrderId);

        if (order != null)
        {

            order.MarkAsPaid();

            await dbContext.SaveChangesAsync();
        }
    }
}
namespace Payment.Models;

public enum PaymentStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
}

public class PaymentTransaction
{
    // 2. Private Setters: Lock getters to prevent external modification, ensuring immutability after creation.
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public string PaymentMethod { get; private set; } = default!;
    public PaymentStatus Status { get; private set; }
    public string? GatewayTransactionId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModified { get; private set; }

    // private constructor to enforce the use of factory method for object creation
    private PaymentTransaction() { }

    // Only allown creattion payment transaction through factory method
    // When a new payment transaction is created, it will be in pending status
    public static PaymentTransaction Create(Guid orderId, decimal amount, string paymentMethod)
    {
        return new PaymentTransaction
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Amount = amount,
            PaymentMethod = paymentMethod,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Behavior Method
    // Not allow edit status directly
    // Must have specific method to change status
    public void MarkAsCompleted(string gatewayTransactionId)
    {
        Status = PaymentStatus.Completed;
        GatewayTransactionId = gatewayTransactionId;
        LastModified = DateTime.UtcNow;
    }

    public void MarkAsFailed(string? gatewayTransactionId = null)
    {
        Status = PaymentStatus.Failed;
        GatewayTransactionId = gatewayTransactionId;
        LastModified = DateTime.UtcNow;
    }
}
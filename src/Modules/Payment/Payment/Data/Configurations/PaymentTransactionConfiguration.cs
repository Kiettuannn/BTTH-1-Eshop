using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Models;

namespace Payment.Data.Configurations;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.HasKey(pt => pt.Id);

        // Index on OrderId
        builder.HasIndex(pt => pt.OrderId);

        builder.Property(pt => pt.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(pt => pt.PaymentMethod)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(pt => pt.GatewayTransactionId)
            .HasMaxLength(100);
    }
}
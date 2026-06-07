using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Payment.Data;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options) { }

    // Ví dụ: Bạn sẽ khai báo các Dbset ở đây sau
    // public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("payment");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
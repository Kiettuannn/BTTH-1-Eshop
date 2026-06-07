using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Data;
using Shared.Data;
using Shared.Data.Interceptors;

namespace Payment;

public static class PaymentModule
{
    public static IServiceCollection AddPaymentModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Data - Infrastructure services
        var connectionString = configuration.GetConnectionString("Database");

        // Đăng ký Interceptors để tự động gán CreatedAt, LastModified và xử lý Domain Event
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<PaymentDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static IApplicationBuilder UsePaymentModule(this IApplicationBuilder app)
    {
        // Chạy tự động Migration để tạo Database Schema cho module này khi app khởi động
        app.UseMigration<PaymentDbContext>();

        return app;
    }
}
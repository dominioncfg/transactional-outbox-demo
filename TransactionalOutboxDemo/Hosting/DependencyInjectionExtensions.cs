using Microsoft.EntityFrameworkCore;
using TransactionalOutboxDemo.Infrastructure;

namespace TransactionalOutboxDemo.Hosting;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddOrdersDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OrdersDb");
        IServiceCollection serviceCollection = services.AddDbContext<OrdersDbContext>(options =>
            options
                .UseNpgsql(connectionString)
            );
        return services;
    }

    public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
    {
        var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
        using var serviceScope = serviceFactory.CreateScope();
        MigrateOrdersDatabaseAsync(serviceScope).GetAwaiter().GetResult();

        return app;
    }

    private static async Task MigrateOrdersDatabaseAsync(IServiceScope serviceScope)
    {
        //Not recomended for production, is best to use CI-CD for this
        await serviceScope.ServiceProvider.GetRequiredService<OrdersDbContext>().Database.MigrateAsync();
    }

}


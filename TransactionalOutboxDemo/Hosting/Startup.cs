using MassTransit;
using MediatR;
using System.Reflection;
using TransactionalOutboxDemo.Domain;
using TransactionalOutboxDemo.Infrastructure;

namespace TransactionalOutboxDemo.Hosting;

public class Startup
{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public void ConfigureServices(IServiceCollection services)
    {
        var executingAssambly = Assembly.GetExecutingAssembly();

        services.AddSingleton<List<Order>>();
        services.AddTransient<IOrderRepository, EfCoreOrderRepository>();
        services.AddMediatR(executingAssambly);

        services.AddMassTransit(x =>
        {
            x.AddConsumers(executingAssambly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });

        });

        services.AddOrdersDbContext(_configuration);

        services.AddSwaggerGen();

        services
            .AddControllers();
    }

    public void Configure(IApplicationBuilder app)
    {
        app
            .InitializeDatabase()
            .UseRouting()
            .UseSwagger()
            .UseSwaggerUI()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
    }
}

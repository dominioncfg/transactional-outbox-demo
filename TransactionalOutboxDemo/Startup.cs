using MassTransit;
using MediatR;
using System.Reflection;
using TransactionalOutboxDemo.Domain;
using TransactionalOutboxDemo.Infrastructure;

namespace TransactionalOutboxDemo;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var executingAssambly = Assembly.GetExecutingAssembly();

        services.AddSingleton<List<Order>>();
        services.AddTransient<IOrderRepository, InMemoryOrderRepository>();
        services.AddMediatR(executingAssambly);

        services.AddMassTransit(x =>
        {
            x.AddConsumers(executingAssambly);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });

        });

        //services.AddMassTransitHostedService();
        services.AddSwaggerGen();

        services
            .AddControllers();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI()
        .UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}

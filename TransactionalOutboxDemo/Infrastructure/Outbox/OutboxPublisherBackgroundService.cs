using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace TransactionalOutboxDemo.Infrastructure;

public class OutboxPublisherBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxPublisherBackgroundService> _logger;
    private volatile bool _ready = false;

    public OutboxPublisherBackgroundService(IServiceScopeFactory scopeFactory, IHostApplicationLifetime lifetime, ILogger<OutboxPublisherBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;

        lifetime.ApplicationStarted.Register(() => _ready = true);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!_ready)
        {
            await Task.Delay(1_000);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await PublishBatchOfPendingMessages(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task PublishBatchOfPendingMessages(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            var bus = scope.ServiceProvider.GetRequiredService<IBus>();

            var events = await dbContext
                .OutboxMessages
                .OrderBy(x => x.Id)
                .Take(50)
                .ToListAsync(stoppingToken);

            _logger.LogInformation("Outbox Sending {NumberMessage} Messages", events.Count);
            foreach (var e in events)
            {
                var type = Type.GetType(e.MessageType) ?? throw new Exception("Type not found");
                var body = JsonSerializer.Deserialize(e.MessagePayload, type) ?? throw new Exception("Fail To deserialize");

                switch (e.DeliveryMode)
                {
                    case MessageDeliveryMode.Publish:
                        await bus.Publish(body, type, stoppingToken);
                        break;
                    case MessageDeliveryMode.Send:
                        await bus.Publish(body, type, stoppingToken);
                        break;
                }

                dbContext.Remove(e);
                dbContext.SaveChanges();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Generic Error - Fail to send outbox messages {Message}", e.Message);
            await Task.Delay(1000, stoppingToken);
        }
    }
}

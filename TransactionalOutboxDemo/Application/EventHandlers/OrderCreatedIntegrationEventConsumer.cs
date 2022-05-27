using MassTransit;
using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Application
{
    public class OrderCreatedIntegrationEventConsumer : IConsumer<OrderCreatedIntegrationEvent>
    {
        private readonly ILogger<OrderCreatedIntegrationEventConsumer> _logger;

        public OrderCreatedIntegrationEventConsumer(ILogger<OrderCreatedIntegrationEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {
            _logger.LogInformation("An order was created with id {Id}", context.Message.Id);
            await Task.Delay(10);
            _logger.LogInformation("After order created", context.Message.Id);
        }
    }
}

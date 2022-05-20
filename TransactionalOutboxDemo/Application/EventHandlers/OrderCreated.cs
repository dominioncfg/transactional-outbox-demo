using MassTransit;
using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Application
{
    public class OrderCreatedDomainEventConsumer : IConsumer<OrderCreatedDomainEvent>
    {
        private readonly ILogger<OrderCreatedDomainEventConsumer> _logger;

        public OrderCreatedDomainEventConsumer(ILogger<OrderCreatedDomainEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedDomainEvent> context)
        {
            _logger.LogInformation("An order was created with id {Id}", context.Message.Id);
            await Task.Delay(10);
            _logger.LogInformation("After order created", context.Message.Id);
        }
    }
}

using MediatR;
using TransactionalOutboxDemo.Domain;
using TransactionalOutboxDemo.Infrastructure;

namespace TransactionalOutboxDemo.Application
{
    public class OrderCreatedDomainEventPublisher : INotificationHandler<OrderCreatedDomainEvent>
    {
        private readonly ILogger<OrderCreatedDomainEventPublisher> _logger;
        private readonly IMessageOutbox _outbox;

        public OrderCreatedDomainEventPublisher(ILogger<OrderCreatedDomainEventPublisher> logger, IMessageOutbox outbox)
        {
            _logger = logger;
            this._outbox = outbox;
        }

        public Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing Integration Event");
            var message = OrderCreatedIntegrationEvent.FromDomainEvent(domainEvent);
            return _outbox.PublishMessageAsync(message, cancellationToken);
        }
    }
}

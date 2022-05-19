namespace TransactionalOutboxDemo.Infrastructure;
public interface IMessageOutbox
{
    Task PublishMessageAsync<T>(T message, CancellationToken cancellationToken);
    Task SendMessageAsync<T>(T message, CancellationToken cancellationToken);
}

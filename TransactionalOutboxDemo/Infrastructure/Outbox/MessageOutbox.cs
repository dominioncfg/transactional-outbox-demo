namespace TransactionalOutboxDemo.Infrastructure;
public class MessageOutbox : IMessageOutbox
{
    private readonly OrdersDbContext _dbContext;

    public MessageOutbox(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task PublishMessageAsync<T>(T message, CancellationToken cancellationToken)
    {
        var sendMessage = OutboxMessagePersistenceModel.FromMessage(MessageDeliveryMode.Publish, message!);
        await _dbContext.OutboxMessages.AddAsync(sendMessage, cancellationToken);
    }

    public async Task SendMessageAsync<T>(T message, CancellationToken cancellationToken)
    {
        var sendMessage = OutboxMessagePersistenceModel.FromMessage(MessageDeliveryMode.Send, message!);
        await _dbContext.OutboxMessages.AddAsync(sendMessage, cancellationToken);
    }

}

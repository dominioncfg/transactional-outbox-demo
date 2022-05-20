namespace TransactionalOutboxDemo.Infrastructure;

public class MessageOutboxFactory : IMessageOutboxFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public MessageOutboxFactory(IServiceProvider sp)
    {
        _serviceProvider = sp;
    }

    public IMessageOutbox Create() => _serviceProvider.GetRequiredService<IMessageOutbox>();
}
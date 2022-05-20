namespace TransactionalOutboxDemo.Infrastructure;

public interface IMessageOutboxFactory
{
    public IMessageOutbox Create();

}

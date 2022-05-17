namespace TransactionalOutboxDemo.Domain;

public interface IOrderRepository
{
    Task Add(Order o, CancellationToken cancellationToken);
}

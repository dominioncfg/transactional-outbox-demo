namespace TransactionalOutboxDemo.Domain;

public interface IUnitOfWork
{
    public void BeginTransaction();
    public Task CompleteAsync(CancellationToken cancellationToken);
}

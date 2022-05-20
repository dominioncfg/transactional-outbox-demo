using Microsoft.EntityFrameworkCore.Storage;
using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Infrastructure;

public class EfCoreUnitOfWork : IUnitOfWork
{
    private readonly OrdersDbContext _ordersDb;
    private IDbContextTransaction? _currentTransaction;
    public EfCoreUnitOfWork(OrdersDbContext ordersDb)
    {
        _ordersDb = ordersDb;
    }

    public void BeginTransaction()
    {
        _currentTransaction = _ordersDb.Database.BeginTransaction();
    }

    public async Task CompleteAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction is null)
            throw new InvalidOperationException("Transaction has not been started");

        await _ordersDb.SaveChangesAsync(cancellationToken);
        await _currentTransaction.CommitAsync(cancellationToken);

        _currentTransaction.Dispose();
        _currentTransaction = null;
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _ordersDb.Dispose();
    }
}

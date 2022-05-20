using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Infrastructure;

public class EfCoreOrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _dbContext;

    public EfCoreOrderRepository(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Order o, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(o, cancellationToken);
    }
}

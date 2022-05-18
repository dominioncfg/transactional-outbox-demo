using MassTransit;
using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Infrastructure;

public class EfCoreOrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _dbContext;
    private readonly IBus _bus;

    public EfCoreOrderRepository(OrdersDbContext dbContext, IBus bus)
    {
        _dbContext = dbContext;
        _bus = bus;
    }

    public async Task Add(Order o, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(o,cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var theEvent in o.DomainEvents)
            await _bus.Publish(theEvent, theEvent.GetType(), cancellationToken);

        o.ClearEvents();
    }
}

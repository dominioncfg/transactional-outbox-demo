using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Infrastructure;

public class EfCoreOrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _dbContext;
    private readonly IMessageOutbox _messageOutbox;

    public EfCoreOrderRepository(OrdersDbContext dbContext, IMessageOutbox messageOutbox)
    {
        _dbContext = dbContext;
        _messageOutbox = messageOutbox;
    }

    public async Task Add(Order o, CancellationToken cancellationToken)
    {
        await _dbContext.AddAsync(o, cancellationToken);
        

        foreach (var theEvent in o.DomainEvents)
            await _messageOutbox.PublishMessageAsync(theEvent, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        o.ClearEvents();
    }
}

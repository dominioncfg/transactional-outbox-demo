using MassTransit;
using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Infrastructure;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<Order> _orders;
    private readonly IBus _bus;

    public InMemoryOrderRepository(List<Order> orders, IBus bus)
    {
        _orders = orders;
        _bus = bus;
    }

    public async Task Add(Order o, CancellationToken cancellationToken)
    {
        _orders.Add(o);

        foreach (var theEvent in o.DomainEvents)
        {
            await _bus.Publish(theEvent, theEvent.GetType(), cancellationToken);
        }

        o.ClearEvents();
    }
}

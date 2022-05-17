namespace TransactionalOutboxDemo.Domain;

public class Order
{
    private readonly List<IDomainEvent> _events = new();
    public Guid Id { get; private set; }
    public Guid BuyerId { get; private set; }
    public int TotalQuantity { get; private set; }
    public decimal TotalPrice { get; private set; }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events;
    public void ClearEvents() => _events.Clear();

    public Order(Guid id, Guid buyerId, int totalQuantity, decimal totalPrice)
    {
        Id = id;
        BuyerId = buyerId;
        TotalQuantity = totalQuantity;
        TotalPrice = totalPrice;
        _events.Add(OrderCreatedDomainEvent.FromOrder(this));
    }

    public void Apply(OrderCreatedDomainEvent @event)
    {
        Id = @event.Id;
        BuyerId = @event.BuyerId;
        TotalQuantity = @event.TotalQuantity;
        TotalPrice = @event.TotalPrice;
    }
}

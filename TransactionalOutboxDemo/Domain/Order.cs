namespace TransactionalOutboxDemo.Domain;
public class Order : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid BuyerId { get; private set; }
    public int TotalQuantity { get; private set; }
    public decimal TotalPrice { get; private set; }

    public Order(Guid id, Guid buyerId, int totalQuantity, decimal totalPrice)
    {
        var createdEvent = new OrderCreatedDomainEvent(id, buyerId, totalQuantity, totalPrice);
        //Validation
        RegisterDomainEvent(createdEvent);
        Apply(createdEvent);
    }

    public void Apply(OrderCreatedDomainEvent @event)
    {
        Id = @event.Id;
        BuyerId = @event.BuyerId;
        TotalQuantity = @event.TotalQuantity;
        TotalPrice = @event.TotalPrice;
    }
}

namespace TransactionalOutboxDemo.Domain;

public class OrderCreatedDomainEvent : IDomainEvent
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public int TotalQuantity { get; init; }
    public decimal TotalPrice { get; init; }

    public OrderCreatedDomainEvent(Guid id, Guid buyerId, int totalQuantity, decimal totalPrice)
    {
        Id = id;
        BuyerId = buyerId;
        TotalQuantity = totalQuantity;
        TotalPrice = totalPrice;
    }
}
namespace TransactionalOutboxDemo.Domain;

public class OrderCreatedIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public int TotalQuantity { get; init; }
    public decimal TotalPrice { get; init; }

    public OrderCreatedIntegrationEvent(Guid id, Guid buyerId, int totalQuantity, decimal totalPrice)
    {
        Id = id;
        BuyerId = buyerId;
        TotalQuantity = totalQuantity;
        TotalPrice = totalPrice;
    }

    public static OrderCreatedIntegrationEvent FromDomainEvent(OrderCreatedDomainEvent domainEvent)
    {
        return new OrderCreatedIntegrationEvent(domainEvent.Id,domainEvent.BuyerId,
                                                domainEvent.TotalQuantity,domainEvent.TotalPrice);
    }
}
namespace TransactionalOutboxDemo.Api.Requests;

public record CreateOrderApiRequest
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public int TotalQuantity { get; init; }
    public decimal TotalPrice { get; init; }
}

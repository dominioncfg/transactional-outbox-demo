using MediatR;
using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Application;

public record CreateOrderCommand : IRequest
{
    public Guid Id { get; init; }
    public Guid BuyerId { get; init; }
    public int TotalQuantity { get; init; }
    public decimal TotalPrice { get; init; }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
    {
        private readonly IOrderRepository _repository;

        public CreateOrderCommandHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(request.Id, request.BuyerId, request.TotalQuantity, request.TotalPrice);

            await _repository.Add(order, cancellationToken);

            return Unit.Value;
        }
    }
}

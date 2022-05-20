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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderRepository _repository;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IOrderRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransaction();
            
            var order = new Order(request.Id, request.BuyerId, request.TotalQuantity, request.TotalPrice);
            await _repository.Add(order, cancellationToken);
            
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}

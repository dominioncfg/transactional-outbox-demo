using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionalOutboxDemo.Api.Requests;
using TransactionalOutboxDemo.Application;

namespace TransactionalOutboxDemo.Api;

[ApiController]
[Route("/api/v1/orders")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateOrderApiRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand()
        {
            Id = request.Id,
            BuyerId = request.BuyerId,
            TotalPrice = request.TotalPrice,
            TotalQuantity = request.TotalQuantity,
        };

        await _mediator.Send(command, cancellationToken);

        return Accepted();
    }

}

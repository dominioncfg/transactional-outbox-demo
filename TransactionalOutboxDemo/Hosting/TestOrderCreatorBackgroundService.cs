using TransactionalOutboxDemo.Application;
using IMediatRMediator = MediatR.IMediator;

namespace TransactionalOutboxDemo.Hosting;

public class TestOrderCreatorBackgroundService : BackgroundService
{
    private readonly ILogger<TestOrderCreatorBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private volatile bool _ready = false;
    private readonly Random _random;

    public TestOrderCreatorBackgroundService(ILogger<TestOrderCreatorBackgroundService> logger, IServiceScopeFactory scopeFactory, IHostApplicationLifetime lifetime)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        lifetime.ApplicationStarted.Register(() => _ready = true);
        _random = new Random();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await WaitUntillApplicationIsFullyStarted();
        await SendMessagesUnlessStopped(stoppingToken);
    }


    private async Task SendMessagesUnlessStopped(CancellationToken cancellationToken)
    {
        var numberOfMessagePerSecond =  50;
        while (!cancellationToken.IsCancellationRequested)
        {
            await CreateRandomOrderAsync(cancellationToken);
            var delay = 1000 / numberOfMessagePerSecond;
            await Task.Delay(delay, cancellationToken);
        }
    }

    private async Task WaitUntillApplicationIsFullyStarted()
    {
        while (!_ready)
            await Task.Delay(1000);
    }

    private async Task CreateRandomOrderAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Sending Message");
            using var scope = _scopeFactory.CreateScope();
            var mediator =  scope.ServiceProvider.GetRequiredService<IMediatRMediator>();


            var cmd = new CreateOrderCommand()
            {
                Id = Guid.NewGuid(),
                BuyerId = Guid.NewGuid(),
                TotalPrice = _random.Next(),
                TotalQuantity = _random.Next(1, 11),
            };
            await mediator.Send(cmd, stoppingToken);
          
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            await Task.Delay(1000, stoppingToken);
        }
    }
}

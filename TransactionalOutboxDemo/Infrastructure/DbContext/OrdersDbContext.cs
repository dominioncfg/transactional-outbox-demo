using Microsoft.EntityFrameworkCore;
using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Infrastructure;

public class OrdersDbContext : DbContext
{
    private readonly IMessageOutboxFactory _messageOutboxFactory;

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OutboxMessagePersistenceModel> OutboxMessages => Set<OutboxMessagePersistenceModel>();

    public OrdersDbContext(DbContextOptions<OrdersDbContext> options, IMessageOutboxFactory messageOutboxFactory) : base(options)
    {
        _messageOutboxFactory = messageOutboxFactory;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AddEntitiesConfiguration(modelBuilder);
    }

    private static void AddEntitiesConfiguration(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessagePersistenceModelEntityTypeConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var messageOutbox = _messageOutboxFactory.Create();
        var entitiesWithEvents = GetEntitiesWithDomainEvents();
        await SendDomainEventsAsync(messageOutbox, entitiesWithEvents, cancellationToken);
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        ClearDomainEvents(entitiesWithEvents);
        return result;
    }

    public override int SaveChanges()
    {
        var affectedRows = SaveChangesAsync(default).GetAwaiter().GetResult();
        return affectedRows;
    }

    private AggregateRoot[] GetEntitiesWithDomainEvents()
    {
        var entitiesWithEvents = ChangeTracker.Entries<AggregateRoot>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        return entitiesWithEvents;
    }

    private static void ClearDomainEvents(AggregateRoot[] entities)
    {
        foreach (var entity in entities)
            entity.ClearEvents();
    }

    private async static Task SendDomainEventsAsync(IMessageOutbox messageOutbox, AggregateRoot[] entities, CancellationToken cancellationToken)
    {
        foreach (var entity in entities)
        {
            foreach (var theEvent in entity.DomainEvents)
            {
                await messageOutbox.PublishMessageAsync(theEvent, cancellationToken);
            }
        }
    }
}

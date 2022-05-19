using Microsoft.EntityFrameworkCore;
using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Infrastructure;

public class OrdersDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OutboxMessagePersistenceModel> OutboxMessages => Set<OutboxMessagePersistenceModel>();

    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    { }

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
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return result;
    }

    public override int SaveChanges()
    {
        return SaveChangesAsync(default).GetAwaiter().GetResult();
    }

}

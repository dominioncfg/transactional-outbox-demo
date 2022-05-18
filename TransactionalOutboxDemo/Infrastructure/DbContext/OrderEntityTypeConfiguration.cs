using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Infrastructure;

internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", "Application");
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.BuyerId).IsRequired();
        
        builder.Property(m=>m.TotalQuantity).IsRequired();
        builder.Property(m => m.TotalPrice).IsRequired();

        builder.Ignore(m => m.DomainEvents);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalOutboxDemo.Domain;

namespace TransactionalOutboxDemo.Infrastructure;

internal class OutboxMessagePersistenceModelEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessagePersistenceModel>
{
    public void Configure(EntityTypeBuilder<OutboxMessagePersistenceModel> builder)
    {
        builder.ToTable("OutboxMessages", "Core");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.Property(m => m.DeliveryMode).IsRequired();

        builder.Property(m => m.MessageType).IsRequired();
        builder.Property(m => m.MessagePayload).IsRequired();
    }
}

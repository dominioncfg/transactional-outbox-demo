// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TransactionalOutboxDemo.Infrastructure;

#nullable disable

namespace TransactionalOutboxDemo.Infrastructure.Migrations
{
    [DbContext(typeof(OrdersDbContext))]
    [Migration("20220518212737_OutboxStorageMigration")]
    partial class OutboxStorageMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TransactionalOutboxDemo.Domain.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BuyerId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("TotalQuantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Orders", "Application");
                });

            modelBuilder.Entity("TransactionalOutboxDemo.Infrastructure.OutboxMessagePersistenceModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DeliveryMode")
                        .HasColumnType("integer");

                    b.Property<string>("MessagePayload")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages", "Core");
                });
#pragma warning restore 612, 618
        }
    }
}

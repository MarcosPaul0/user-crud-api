using AutoriaStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoriaStore.Infrastructure.EntitiesConfiguration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("order");

        builder.HasKey(order => order.Id);

        builder.Property(order => order.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(order => order.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(order => order.TotalPriceInCents).HasColumnName("total_price_in_cents").IsRequired();
        builder.Property(order => order.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
        builder.Property(order => order.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();

        builder.HasOne(order => order.User)
            .WithMany()
            .HasForeignKey(order => order.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(order => order.ProductOrders)
            .WithOne(orderProduct => orderProduct.Order)
            .HasForeignKey(orderProduct => orderProduct.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

// <copyright file="OrderProductConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoriaStore.Infrastructure.EntitiesConfiguration;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.ToTable("order_product");

        builder.HasKey(orderProduct => orderProduct.Id);

        builder.Property(orderProduct => orderProduct.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(orderProduct => orderProduct.ProductName).HasColumnName("product_name").HasMaxLength(255).IsRequired();
        builder.Property(orderProduct => orderProduct.Quantity).HasColumnName("quantity").IsRequired();
        builder.Property(orderProduct => orderProduct.UnitPriceInCents).HasColumnName("unit_price_in_cents").IsRequired();
        builder.Property(orderProduct => orderProduct.TotalPriceInCents).HasColumnName("total_price_in_cents").IsRequired();
        builder.Property(orderProduct => orderProduct.OrderId).HasColumnName("order_id").IsRequired();
        builder.Property(orderProduct => orderProduct.ProductId).HasColumnName("product_id").IsRequired();
        builder.Property(orderProduct => orderProduct.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
        builder.Property(orderProduct => orderProduct.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();

        builder.HasOne(orderProduct => orderProduct.Order)
            .WithMany(order => order.ProductOrders)
            .HasForeignKey(orderProduct => orderProduct.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(orderProduct => orderProduct.Product)
            .WithMany()
            .HasForeignKey(orderProduct => orderProduct.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

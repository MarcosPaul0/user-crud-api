// <copyright file="OrderConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
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
        builder.Property(order => order.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(OrderStatus.AwaitingPayment)
            .IsRequired();
        builder.Property(order => order.PaymentStatus)
            .HasColumnName("payment_status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue(OrderPaymentStatus.Pending)
            .IsRequired();
        builder.Property(order => order.PaymentProvider).HasColumnName("payment_provider").HasMaxLength(50).IsRequired();
        builder.Property(order => order.PaymentMethod).HasColumnName("payment_method").HasMaxLength(50).IsRequired();
        builder.Property(order => order.PaymentId).HasColumnName("payment_id").HasMaxLength(100).IsRequired(false);
        builder.Property(order => order.PaymentExternalId).HasColumnName("payment_external_id").HasMaxLength(100).IsRequired(false);
        builder.Property(order => order.PixCopyPasteCode).HasColumnName("pix_copy_paste_code").HasColumnType("text").IsRequired(false);
        builder.Property(order => order.PixQrCodeBase64).HasColumnName("pix_qr_code_base64").HasColumnType("text").IsRequired(false);
        builder.Property(order => order.PaymentExpiresAt).HasColumnName("payment_expires_at").IsRequired(false);
        builder.Property(order => order.PaidAt).HasColumnName("paid_at").IsRequired(false);
        builder.Property(order => order.RefundedAt).HasColumnName("refunded_at").IsRequired(false);
        builder.Property(order => order.DisputedAt).HasColumnName("disputed_at").IsRequired(false);
        builder.Property(order => order.CancelledAt).HasColumnName("cancelled_at").IsRequired(false);
        builder.Property(order => order.ReceiptUrl).HasColumnName("receipt_url").HasColumnType("text").IsRequired(false);
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

        builder.HasIndex(order => order.PaymentId).IsUnique();
    }
}

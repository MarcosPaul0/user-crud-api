// <copyright file="ProductImageConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoriaStore.Infrastructure.EntitiesConfiguration;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.HasKey(productImage => productImage.Id);

        builder.ToTable("product_image", table =>
        {
            table.HasCheckConstraint(
                "CK_ProductImage_DisplayOrder",
                "display_order BETWEEN 1 AND 5");
        });

        builder.Property(productImage => productImage.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(productImage => productImage.ImageUrl).HasColumnName("image_url").HasMaxLength(500).IsRequired();
        builder.Property(productImage => productImage.DisplayOrder).HasColumnName("display_order").IsRequired();
        builder.Property(productImage => productImage.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
        builder.Property(productImage => productImage.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();

        builder.Property(productImage => productImage.ProductId).HasColumnName("product_id").IsRequired();
        builder.HasOne(productImage => productImage.Product)
            .WithMany(product => product.ProductImages)
            .HasForeignKey(productImage => productImage.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
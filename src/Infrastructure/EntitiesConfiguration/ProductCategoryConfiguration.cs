// <copyright file="ProductCategoryConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoriaStore.Infrastructure.EntitiesConfiguration;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(productCategory => productCategory.Id);

        builder.ToTable("product_category");

        builder.Property(productCategory => productCategory.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(productCategory => productCategory.Category).HasColumnName("category").HasMaxLength(50).IsRequired();
        builder.Property(productCategory => productCategory.IsActive).HasColumnName("is_active").HasDefaultValue(true).IsRequired();
        builder.Property(productCategory => productCategory.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
        builder.Property(productCategory => productCategory.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();
    }
}
// <copyright file="UserConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoriaStore.Infrastructure.EntitiesConfiguration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(user => user.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(user => user.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        builder.Property(user => user.Password).HasColumnName("password").HasMaxLength(255).IsRequired();
        builder.Property(user => user.Role).HasColumnName("role").HasDefaultValue(UserRole.Customer).HasSentinel(UserRole.None).IsRequired();
        builder.Property(user => user.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAdd().IsRequired();
        builder.Property(user => user.UpdatedAt).HasColumnName("updated_at").IsRequired(false).ValueGeneratedOnUpdate();

        builder.HasIndex(user => user.Email).IsUnique();
    }
}
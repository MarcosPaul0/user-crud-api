// <copyright file="ApplicationDbContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoriaStore.Infrastructure.Context;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }

    public DbSet<Phone> Phone { get; set; }

    public DbSet<ProductCategory> ProductCategory { get; set; }

    public DbSet<Product> Product { get; set; }

    public DbSet<ProductImage> ProductImage { get; set; }

    public DbSet<Order> Order { get; set; }

    public DbSet<OrderProduct> OrderProduct { get; set; }

    public DbSet<IdempotencyKey> IdempotencyKey { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Aplica as configurações das entidades definidas que estendem de IEntityTypeConfiguration<>
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

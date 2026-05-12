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
        // // 1. Aplica as configurações primeiro para descobrir todas as propriedades e tipos
        // builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        //
        // // 2. Agora o builder.Model já contém as informações necessárias
        // var enumTypes = builder.Model.GetEntityTypes()
        //     .SelectMany(e => e.GetProperties())
        //     .Select(p => p.ClrType)
        //     .Select(t => Nullable.GetUnderlyingType(t) ?? t)
        //     .Where(t => t.IsEnum)
        //     .Distinct();
        //
        // var genericMethod = typeof(NpgsqlModelBuilderExtensions)
        //     .GetMethods(BindingFlags.Public | BindingFlags.Static)
        //     .FirstOrDefault(m => m is { Name: nameof(NpgsqlModelBuilderExtensions.HasPostgresEnum), IsGenericMethod: true }
        //                          && m.GetParameters().Length == 4);
        //
        // var translator = new NpgsqlSnakeCaseNameTranslator();
        //
        // foreach (var enumType in enumTypes)
        // {
        //     genericMethod?
        //         .MakeGenericMethod(enumType)
        //         .Invoke(null, [builder, null, null, translator]);
        // }
        base.OnModelCreating(builder);

        // Aplica as configurações das entidades definidas que estendem de IEntityTypeConfiguration<>
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

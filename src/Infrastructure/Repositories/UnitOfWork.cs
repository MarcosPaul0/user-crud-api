// <copyright file="UnitOfWork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace AutoriaStore.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;

    private readonly Lazy<IUserRepository> user;
    private readonly Lazy<IProductRepository> product;
    private readonly Lazy<IProductCategoryRepository> productCategory;
    private readonly Lazy<IProductImageRepository> productImage;
    private readonly Lazy<IOrderRepository> order;
    private readonly Lazy<IIdempotencyKeyRepository> idempotencyKey;

    public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        this.context = context;

        this.user = new Lazy<IUserRepository>(
            serviceProvider.GetRequiredService<IUserRepository>);
        this.product = new Lazy<IProductRepository>(
            serviceProvider.GetRequiredService<IProductRepository>);
        this.productCategory = new Lazy<IProductCategoryRepository>(
            serviceProvider.GetRequiredService<IProductCategoryRepository>);
        this.productImage = new Lazy<IProductImageRepository>(
            serviceProvider.GetRequiredService<IProductImageRepository>);
        this.order = new Lazy<IOrderRepository>(
            serviceProvider.GetRequiredService<IOrderRepository>);
        this.idempotencyKey = new Lazy<IIdempotencyKeyRepository>(
            serviceProvider.GetRequiredService<IIdempotencyKeyRepository>);
    }

    public IUserRepository User => this.user.Value;

    public IProductRepository Product => this.product.Value;

    public IProductCategoryRepository ProductCategory => this.productCategory.Value;

    public IProductImageRepository ProductImage => this.productImage.Value;

    public IOrderRepository Order => this.order.Value;

    public IIdempotencyKeyRepository IdempotencyKey => this.idempotencyKey.Value;

    public async Task SaveChangesAsync()
    {
        await this.context.SaveChangesAsync();
    }

    public void Dispose()
    {
        this.context.Dispose();
        GC.SuppressFinalize(this);
    }
}

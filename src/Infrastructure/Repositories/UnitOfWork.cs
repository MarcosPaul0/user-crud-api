// <copyright file="UnitOfWork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace AutoriaStore.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private readonly Lazy<IUserRepository> _user;
    private readonly Lazy<IProductRepository> _product;
    private readonly Lazy<IProductCategoryRepository> _productCategory;
    private readonly Lazy<IProductImageRepository> _productImage;
    private readonly Lazy<IOrderRepository> _order;
    private readonly Lazy<IIdempotencyKeyRepository> _idempotencyKey;

    public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        this._context = context;

        this._user = new Lazy<IUserRepository>(
            serviceProvider.GetRequiredService<IUserRepository>);
        this._product = new Lazy<IProductRepository>(
            serviceProvider.GetRequiredService<IProductRepository>);
        this._productCategory = new Lazy<IProductCategoryRepository>(
            serviceProvider.GetRequiredService<IProductCategoryRepository>);
        this._productImage = new Lazy<IProductImageRepository>(
            serviceProvider.GetRequiredService<IProductImageRepository>);
        this._order = new Lazy<IOrderRepository>(
            serviceProvider.GetRequiredService<IOrderRepository>);
        this._idempotencyKey = new Lazy<IIdempotencyKeyRepository>(
            serviceProvider.GetRequiredService<IIdempotencyKeyRepository>);
    }

    public IUserRepository User => this._user.Value;

    public IProductRepository Product => this._product.Value;

    public IProductCategoryRepository ProductCategory => this._productCategory.Value;

    public IProductImageRepository ProductImage => this._productImage.Value;

    public IOrderRepository Order => this._order.Value;

    public IIdempotencyKeyRepository IdempotencyKey => this._idempotencyKey.Value;

    public async Task SaveChangesAsync()
    {
        await this._context.SaveChangesAsync();
    }

    public void Dispose()
    {
        this._context.Dispose();
        GC.SuppressFinalize(this);
    }
}

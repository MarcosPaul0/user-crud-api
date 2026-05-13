// <copyright file="IUnitOfWork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository User { get; }

    IProductRepository Product { get; }

    IProductCategoryRepository ProductCategory { get; }

    IProductImageRepository ProductImage { get; }

    IOrderRepository Order { get; }

    IIdempotencyKeyRepository IdempotencyKey { get; }

    Task SaveChangesAsync();
}

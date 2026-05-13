// <copyright file="IProductCategoryRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces.Repositories;

public interface IProductCategoryRepository : IBaseRepository<ProductCategory>
{
    Task<ProductCategory?> FindByCategoryAsync(string category, CancellationToken cancellationToken = default);

    Task<List<ProductCategory>> FindAllWithProductCountAsync(
        ProductCategory? filter,
        CancellationToken cancellationToken = default);

    Task<List<ProductCategory>> FindAllAsync(
        ProductCategory? filter,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
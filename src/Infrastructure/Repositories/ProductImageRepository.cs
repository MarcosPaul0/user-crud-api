// <copyright file="ProductImageRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AutoriaStore.Infrastructure.Repositories;

public class ProductImageRepository(ApplicationDbContext context) : BaseRepository<ProductImage>(context), IProductImageRepository
{
    private readonly ApplicationDbContext context = context;

    public async Task<List<ProductImage>> FindAllByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await this.context.ProductImage.AsNoTracking().Where(productImage => productImage.ProductId == productId)
            .ToListAsync(cancellationToken);
    }
}
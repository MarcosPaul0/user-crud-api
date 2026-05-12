// <copyright file="ListProductCategoryUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.ListProductCategory;

public sealed class ListProductCategoryUseCase(
    IUnitOfWork unitOfWork) : IListProductCategoryUseCase
{
    public async Task<(List<ProductCategory> productCategories, int count)> ExecuteAsync(CancellationToken cancellationToken)
    {
        var filter = new ProductCategory()
        {
            IsActive = true,
        };

        var productCategories = await unitOfWork.ProductCategory.FindAllAsync(filter, cancellationToken);

        return (productCategories, productCategories.Count());
    }
}
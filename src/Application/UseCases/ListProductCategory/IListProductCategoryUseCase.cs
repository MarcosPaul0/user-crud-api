// <copyright file="IListProductCategoryUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.ListProductCategory;

public interface IListProductCategoryUseCase
{
    Task<(List<ProductCategory> productCategories, int count)> ExecuteAsync(CancellationToken cancellationToken);
}
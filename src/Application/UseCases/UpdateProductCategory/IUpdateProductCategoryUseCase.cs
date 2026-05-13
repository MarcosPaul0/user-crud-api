// <copyright file="IUpdateProductCategoryUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.UpdateProductCategory;

public interface IUpdateProductCategoryUseCase
{
    Task ExecuteAsync(
        Guid productCategoryId,
        UpdateProductCategoryDto updateProductCategoryDto,
        CancellationToken cancellationToken);
}
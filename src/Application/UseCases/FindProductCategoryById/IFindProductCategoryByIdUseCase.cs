// <copyright file="IFindProductCategoryByIdUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.FindProductCategoryById;

public interface IFindProductCategoryByIdUseCase
{
    Task<ProductCategory> ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken);
}
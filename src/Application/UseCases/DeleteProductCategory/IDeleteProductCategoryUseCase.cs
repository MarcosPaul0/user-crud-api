// <copyright file="IDeleteProductCategoryUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.UseCases.DeleteProductCategory;

public interface IDeleteProductCategoryUseCase
{
    Task ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken);
}
// <copyright file="IDeleteProductUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.UseCases.DeleteProduct;

public interface IDeleteProductUseCase
{
    Task ExecuteAsync(Guid productId, CancellationToken cancellationToken);
}
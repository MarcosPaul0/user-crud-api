// <copyright file="IFindProductByIdUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.FindProductById;

public interface IFindProductByIdUseCase
{
    Task<Product> ExecuteAsync(Guid productId, CancellationToken cancellationToken);
}
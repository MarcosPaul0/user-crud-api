// <copyright file="ICreateProductUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.CreateProduct;

public interface ICreateProductUseCase
{
    Task ExecuteAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);
}
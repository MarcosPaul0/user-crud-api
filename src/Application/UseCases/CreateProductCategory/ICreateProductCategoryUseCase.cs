// <copyright file="ICreateProductCategoryUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.CreateProductCategory;

public interface ICreateProductCategoryUseCase
{
    Task ExecuteAsync(CreateProductCategoryDto createProductCategoryDto, CancellationToken cancellationToken);
}
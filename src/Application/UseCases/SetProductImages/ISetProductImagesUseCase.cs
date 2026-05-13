// <copyright file="ISetProductImagesUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.SetProductImages;

public interface ISetProductImagesUseCase
{
    Task ExecuteAsync(Guid productId, SetProductImagesDto setProductImagesDto, CancellationToken cancellationToken);
}
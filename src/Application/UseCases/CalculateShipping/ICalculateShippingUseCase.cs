// <copyright file="ICalculateShippingUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.CalculateShipping;

public interface ICalculateShippingUseCase
{
    Task<CalculateShippingResultDto> ExecuteAsync(
        CalculateShippingDto calculateShippingDto,
        CancellationToken cancellationToken);
}
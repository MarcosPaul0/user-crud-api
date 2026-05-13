// <copyright file="ICreateOrderUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.CreateOrder;

public interface ICreateOrderUseCase
{
    Task<CreateOrderResultDto> ExecuteAsync(
        CreateOrderDto createOrderDto,
        string idempotencyKey,
        string endpoint,
        CancellationToken cancellationToken);
}

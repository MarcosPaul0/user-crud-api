// <copyright file="IFindOrderByIdUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.FindOrderById;

public interface IFindOrderByIdUseCase
{
    Task<OrderDetailsDto> ExecuteAsync(Guid orderId, CancellationToken cancellationToken);
}

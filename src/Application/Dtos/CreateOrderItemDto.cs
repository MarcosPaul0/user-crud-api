// <copyright file="CreateOrderItemDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Dtos;

public sealed record CreateOrderItemDto
{
    required public Guid ProductId { get; init; }
    required public int Quantity { get; init; }
}

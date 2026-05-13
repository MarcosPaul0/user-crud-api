// <copyright file="CreateOrderDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public sealed record CreateOrderDto
{
    [MinLength(1)]
    required public IReadOnlyCollection<CreateOrderItemDto> Items { get; init; }
}

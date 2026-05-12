// <copyright file="OrderItemSummaryDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Dtos;

public sealed record OrderItemSummaryDto
{
    required public Guid ProductId { get; init; }
    required public string ProductName { get; init; }
    required public int Quantity { get; init; }
    required public int UnitPriceInCents { get; init; }
    required public int TotalPriceInCents { get; init; }
}

// <copyright file="ProductListResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.API.Dtos;

public record ProductListResponseDto
{
    required public Guid Id { get; init; }
    required public string Name { get; init; }
    required public int PriceInCents { get; init; }
    required public byte DiscountPercentage { get; init; }
    required public Guid ProductCategoryId { get; init; }
    required public string Category { get; init; }
    required public ProductImageResponseDto ProductImage { get; init; }
}
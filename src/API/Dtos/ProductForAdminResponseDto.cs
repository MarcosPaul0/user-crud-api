// <copyright file="ProductForAdminResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.API.Dtos;

public record ProductForAdminResponseDto
{
    required public Guid Id { get; init; }
    required public string Name { get; init; }
    required public string PrintDescription { get; init; }
    required public string Description { get; init; }
    required public int PriceInCents { get; init; }
    required public int ProductionTimeInMinutes { get; init; }
    required public byte DiscountPercentage { get; init; }
    required public bool? IsActive { get; init; }
    required public int StockQuantity { get; init; }
    required public int DepthInCentimeters { get; init; }
    required public int WidthInCentimeters { get; init; }
    required public int HeightInCentimeters { get; init; }
    required public int WeightInGrams { get; init; }
    required public Guid ProductCategoryId { get; init; }
    required public string Category { get; init; }
    required public List<ProductImageResponseDto> ProductImages { get; init; }
    required public DateTime CreatedAt { get; init; }
    required public DateTime? UpdatedAt { get; init; }
}
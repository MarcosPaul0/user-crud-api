// <copyright file="UpdateProductDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public record UpdateProductDto
{
    [StringLength(100, MinimumLength = 10)]
    public string? Name { get; init; }

    [StringLength(1200, MinimumLength = 10)]
    public string? Description { get; init; }

    [StringLength(600, MinimumLength = 10)]
    public string? PrintDescription { get; init; }

    [Range(1, int.MaxValue)]
    public int? PriceInCents { get; init; }

    [Range(1, int.MaxValue)]
    public int? ProductionTimeInMinutes { get; init; }

    [Range(1, int.MaxValue)]
    public int? StockQuantity { get; init; }

    [Range(0, 30)]
    public byte? DiscountPercentage { get; init; }

    [Range(1, 100)]
    public int? DepthInCentimeters { get; set; }

    [Range(1, 100)]
    public int? WidthInCentimeters { get; set; }

    [Range(1, 100)]
    public int? HeightInCentimeters { get; set; }

    [Range(1, 10_000)]
    public int? WeightInGrams { get; set; }

    public Guid? ProductCategoryId { get; init; }

    public bool? IsActive { get; init; }
}
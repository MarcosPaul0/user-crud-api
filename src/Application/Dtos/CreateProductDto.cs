// <copyright file="CreateProductDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public class CreateProductDto
{
    [Required]
    [StringLength(100, MinimumLength = 10)]
    public string Name { get; init; }

    [Required]
    [StringLength(600, MinimumLength = 10)]
    public string PrintDescription { get; init; }

    [Required]
    [StringLength(1_200, MinimumLength = 10)]
    public string Description { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int PriceInCents { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ProductionTimeInMinutes { get; init; }

    [Required]
    [Range(0, 30)]
    public byte DiscountPercentage { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int StockQuantity { get; init; }

    [Required]
    public Guid ProductCategoryId { get; init; }

    [Required]
    [Range(1, 100)]
    public int DepthInCentimeters { get; set; }

    [Required]
    [Range(1, 100)]
    public int WidthInCentimeters { get; set; }

    [Required]
    [Range(1, 100)]
    public int HeightInCentimeters { get; set; }

    [Required]
    [Range(1, 10_000)]
    public int WeightInGrams { get; set; }
}
// <copyright file="Product.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Entities;

public class Product : Entity
{
    public string Name { get; set; } = null!;

    public string PrintDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int PriceInCents { get; set; }

    public int ProductionTimeInMinutes { get; set; }

    public byte DiscountPercentage { get; set; }

    public bool? IsActive { get; set; }

    public int StockQuantity { get; set; }

    public int DepthInCentimeters { get; set; }

    public int WidthInCentimeters { get; set; }

    public int HeightInCentimeters { get; set; }

    public int WeightInGrams { get; set; }

    public Guid ProductCategoryId { get; set; }

    public ProductCategory ProductCategory { get; set; } = null!;

    public List<ProductImage> ProductImages { get; set; } = null!;
}
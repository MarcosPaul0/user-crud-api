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

    // public Product(
    //     string name,
    //     string description,
    //     string printDescription,
    //     int priceInCents,
    //     int productionTimeInMinutes,
    //     byte discountPercentage,
    //     int stockQuantity,
    //     Guid productCategoryId,
    //     DateTime createdAt)
    // {
    //     Name = name;
    //     Description = description;
    //     PriceInCents = priceInCents;
    //     PrintDescription = printDescription;
    //     ProductionTimeInMinutes = productionTimeInMinutes;
    //     IsActive = true;
    //     DiscountPercentage = discountPercentage;
    //     StockQuantity = stockQuantity;
    //     ProductCategoryId = productCategoryId;
    //     CreatedAt = createdAt;
    // }
    //
    // public Product(
    //     Guid id,
    //     string name,
    //     string printDescription,
    //     string description,
    //     int priceInCents,
    //     int productionTimeInMinutes,
    //     bool? isActive,
    //     byte discountPercentage,
    //     int stockQuantity,
    //     Guid productCategoryId,
    //     ProductCategory productCategory,
    //     List<ProductImage> productImages,
    //     DateTime createdAt,
    //     DateTime? updatedAt)
    // {
    //     Id = id;
    //     Name = name;
    //     PrintDescription = printDescription;
    //     Description = description;
    //     PriceInCents = priceInCents;
    //     ProductionTimeInMinutes = productionTimeInMinutes;
    //     IsActive = isActive;
    //     DiscountPercentage = discountPercentage;
    //     StockQuantity = stockQuantity;
    //     ProductCategoryId = productCategoryId;
    //     ProductCategory = productCategory;
    //     ProductImages = productImages;
    //     CreatedAt = createdAt;
    //     UpdatedAt = updatedAt;
    // }
}
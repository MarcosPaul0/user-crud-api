// <copyright file="ProductImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Entities;

public class ProductImage : Entity
{
    public string ImageUrl { get; set; }

    public byte DisplayOrder { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public ProductImage(string imageUrl, byte displayOrder, Guid productId, DateTime createdAt)
    {
        this.ImageUrl = imageUrl;
        this.DisplayOrder = displayOrder;
        this.ProductId = productId;
        this.CreatedAt = createdAt;
    }
}
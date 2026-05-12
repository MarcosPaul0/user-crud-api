// <copyright file="OrderProduct.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Entities;

public class OrderProduct : Entity
{
    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public int UnitPriceInCents { get; set; }

    public int TotalPriceInCents { get; set; }

    public Guid OrderId { get; set; }

    public Order Order { get; set; } = null!;

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;
}

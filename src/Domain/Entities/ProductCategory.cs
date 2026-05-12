// <copyright file="ProductCategory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AutoriaStore.Domain.Entities;

public class ProductCategory : Entity
{
    public string Category { get; set; } = null!;

    public bool IsActive { get; set; }

    [NotMapped]
    [JsonIgnore]
    public int ProductCount { get; set; }
}
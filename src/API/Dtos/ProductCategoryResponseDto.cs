// <copyright file="ProductCategoryResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.API.Dtos;

public record ProductCategoryResponseDto
{
    required public Guid Id { get; init; }
    required public string Category { get; init; }
    required public DateTime CreatedAt { get; init; }
}
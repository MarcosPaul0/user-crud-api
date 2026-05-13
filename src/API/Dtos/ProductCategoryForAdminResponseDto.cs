// <copyright file="ProductCategoryForAdminResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.API.Dtos;

public record ProductCategoryForAdminResponseDto
{
    required public Guid Id { get; init; }
    required public string Category { get; init; }
    required public bool IsActive { get; init; }
    required public int ProductCount { get; init; }
    required public DateTime CreatedAt { get; init; }
}
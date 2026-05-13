// <copyright file="ProductImageResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.API.Dtos;

public record ProductImageResponseDto
{
    required public Guid Id { get; init; }
    required public string ImageUrl { get; init; }
    required public byte DisplayOrder { get; init; }
    required public DateTime CreatedAt { get; init; }
    required public DateTime? UpdatedAt { get; init; }
}
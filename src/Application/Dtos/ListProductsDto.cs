// <copyright file="ListProductsDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public record ListProductsDto : PaginationDto
{
    [StringLength(100, MinimumLength = 3)]
    public string? Name { get; init; }

    public Guid? ProductCategoryId { get; init; }
}
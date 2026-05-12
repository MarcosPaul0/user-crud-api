// <copyright file="SetProductImagesDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public record SetProductImagesDto
{
    [Required]
    public List<ProductImageDto> Images { get; init; }
}
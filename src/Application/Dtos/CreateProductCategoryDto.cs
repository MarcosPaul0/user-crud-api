// <copyright file="CreateProductCategoryDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public class CreateProductCategoryDto
{
    [Required]
    [StringLength(50, MinimumLength = 10)]
    public string Category { get; init; }
}
// <copyright file="ProductImageDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using AutoriaStore.Application.Attributes;
using Microsoft.AspNetCore.Http;

namespace AutoriaStore.Application.Dtos;

public class ProductImageDto
{
    public Guid? Id { get; init; }

    [AllowedImageExtensions(".jpg", ".jpeg", ".png", ".webp")]
    [MaxFileSizeAttribute(3)]
    public IFormFile? File { get; init; }

    [Required]
    [Range(1, 4)]
    public byte DisplayOrder { get; init; }
}
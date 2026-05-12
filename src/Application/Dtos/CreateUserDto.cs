// <copyright file="CreateUserDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public record CreateUserDto
{
    [Required]
    [StringLength(50, MinimumLength = 10)]
    public string Name { get; init; }

    [Required]
    [EmailAddress]
    [StringLength(255, MinimumLength = 1)]
    public string Email { get; init; }

    [Required]
    [MinLength(10)]
    [StringLength(50, MinimumLength = 10)]
    public string Password { get; init; }
}
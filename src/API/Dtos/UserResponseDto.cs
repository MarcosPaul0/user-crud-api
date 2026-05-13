// <copyright file="UserResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Enums;

namespace AutoriaStore.API.Dtos;

public record UserResponseDto
{
    required public Guid Id { get; init; }
    required public string Name { get; init; }
    required public string Email { get; init; }
    required public UserRole Role { get; init; }
    required public DateTime CreatedAt { get; init; }
    required public DateTime? UpdatedAt { get; init; }
}
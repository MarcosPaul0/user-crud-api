// <copyright file="ListUsersDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using AutoriaStore.Domain.Enums;

namespace AutoriaStore.Application.Dtos;

public record ListUsersDto : PaginationDto
{
    [StringLength(50, MinimumLength = 3)]
    public string? Name { get; init; }

    [EnumDataType(typeof(UserRole))]
    public UserRole? Role { get; init; }
}
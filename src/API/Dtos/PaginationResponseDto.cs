// <copyright file="PaginationResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.API.Dtos;

public record PaginationResponseDto<T>
{
    required public IEnumerable<T> Items { get; init; }
    required public int TotalPages { get; init; }
    required public int TotalItems { get; init; }
    required public int ItemsPerPage { get; init; }
    required public int Page { get; init; }
    required public bool HasPrevious { get; init; }
    required public bool HasNext { get; init; }
}
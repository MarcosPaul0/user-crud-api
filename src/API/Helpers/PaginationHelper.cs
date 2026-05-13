// <copyright file="PaginationHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.API.Dtos;

namespace AutoriaStore.API.Helpers;

public static class PaginationHelper
{
    public static PaginationResponseDto<T> FormatResponse<T>(IEnumerable<T> items, int totalItems, int page, int itemsPerPage)
    {
        var totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

        var hasNextPage = page + 1 <= totalPages;

        var hasPreviousPage = page - 1 >= 1;

        return new PaginationResponseDto<T>()
        {
            Items = items,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Page = page,
            HasNext = hasNextPage,
            HasPrevious = hasPreviousPage,
            ItemsPerPage = itemsPerPage,
        };
    }
}
// <copyright file="ProductCategoryForAdminPresenter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.API.Dtos;
using AutoriaStore.Domain.Entities;

namespace AutoriaStore.API.Presenters;

public static class ProductCategoryForAdminPresenter
{
    public static ProductCategoryForAdminResponseDto ToHttp(ProductCategory productCategory)
    {
        return new ProductCategoryForAdminResponseDto
        {
            Id = productCategory.Id,
            Category = productCategory.Category,
            IsActive = productCategory.IsActive,
            CreatedAt = productCategory.CreatedAt,
            ProductCount = productCategory.ProductCount,
        };
    }

    public static PaginationResponseDto<ProductCategoryForAdminResponseDto> ToHttp(IEnumerable<ProductCategory> products, int count)
    {
        var productCategoriesResponse = products.Select(ToHttp);

        return new PaginationResponseDto<ProductCategoryForAdminResponseDto>()
        {
            HasNext = false,
            HasPrevious = false,
            Items = productCategoriesResponse,
            ItemsPerPage = count,
            TotalItems = count,
            Page = 1,
            TotalPages = 1,
        };
    }
}
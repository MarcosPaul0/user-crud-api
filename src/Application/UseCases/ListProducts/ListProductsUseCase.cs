// <copyright file="ListProductsUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.ListProducts;

public sealed class ListProductsUseCase(IUnitOfWork unitOfWork) : IListProductsUseCase
{
    public async Task<(IEnumerable<Product> products, int count)> ExecuteAsync(
        ListProductsDto listProductsDto,
        CancellationToken cancellationToken)
    {
        var productFilter = new Product()
        {
            IsActive = true,
            ProductCategory = new ProductCategory()
            {
                IsActive = true,
            },
        };

        if (!string.IsNullOrWhiteSpace(listProductsDto.Name))
        {
            productFilter.Name = listProductsDto.Name;
        }

        if (listProductsDto.ProductCategoryId != null && listProductsDto.ProductCategoryId != Guid.Empty)
        {
            productFilter.ProductCategoryId = listProductsDto.ProductCategoryId.Value;
        }

        if (listProductsDto.ProductCategoryId != null && listProductsDto.ProductCategoryId != Guid.Empty)
        {
            productFilter.ProductCategoryId = listProductsDto.ProductCategoryId.Value;
        }

        var products = await unitOfWork.Product.FindAllAsync(
            productFilter,
            listProductsDto.Page,
            listProductsDto.ItemsPerPage,
            cancellationToken);

        var productsCount = await unitOfWork.Product.CountAsync(productFilter, cancellationToken);

        return (products, productsCount);
    }
}
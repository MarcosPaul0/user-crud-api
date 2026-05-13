// <copyright file="IListProductsForAdminUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.ListProductsForAdmin;

public interface IListProductsForAdminUseCase
{
    Task<(IEnumerable<Product> products, int count)> ExecuteAsync(
        ListProductsByAdminDto listProductsByAdminDto,
        CancellationToken cancellationToken);
}
// <copyright file="IFindProductCategoryByIdForAdminUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.FindProductCategoryByIdForAdmin;

public interface IFindProductCategoryByIdForAdminUseCase
{
    Task<ProductCategory> ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken);
}
// <copyright file="ListProductCategoryForAdminUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.ListProductCategoryForAdmin;

public class ListProductCategoryForAdminUseCase(IUnitOfWork unitOfWork) : IListProductCategoryForAdminUseCase
{
    public async Task<(List<ProductCategory> productCategories, int count)> ExecuteAsync(CancellationToken cancellationToken)
    {
        var productCategories = await unitOfWork.ProductCategory.FindAllAsync(null, cancellationToken);

        return (productCategories, productCategories.Count());
    }
}
// <copyright file="DeleteProductCategoryUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.DeleteProductCategory;

public sealed class DeleteProductCategoryUseCase(IUnitOfWork unitOfWork) : IDeleteProductCategoryUseCase
{
    public async Task ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken)
    {
        var productCategory = await unitOfWork.ProductCategory.FindByIdAsync(productCategoryId, cancellationToken);

        if (productCategory == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND);
        }

        await unitOfWork.ProductCategory.DeleteAsync(productCategory, cancellationToken);

        await unitOfWork.SaveChangesAsync();
    }
}
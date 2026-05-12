// <copyright file="UpdateProductCategoryUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.UpdateProductCategory;

public sealed class UpdateProductCategoryUseCase(IUnitOfWork unitOfWork) : IUpdateProductCategoryUseCase
{
    public async Task ExecuteAsync(
        Guid productCategoryId,
        UpdateProductCategoryDto updateProductCategoryDto,
        CancellationToken cancellationToken)
    {
        var productCategory =
            await unitOfWork.ProductCategory.FindByIdAsync(productCategoryId, cancellationToken);

        if (productCategory == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCTCATEGORYNOTFOUND);
        }

        var isUpdated = false;

        if (!string.IsNullOrWhiteSpace(updateProductCategoryDto.Category))
        {
            var productCategoryAlreadyExists =
                await unitOfWork.ProductCategory.FindByCategoryAsync(updateProductCategoryDto.Category, cancellationToken);

            if (productCategoryAlreadyExists != null && productCategoryAlreadyExists.Id != productCategory.Id)
            {
                throw new ConflictException(ExceptionMessages.PRODUCTCATEGORYALREADYEXISTS);
            }

            productCategory.Category = updateProductCategoryDto.Category;
            isUpdated = true;
        }

        if (updateProductCategoryDto.IsActive is not null)
        {
            productCategory.IsActive = updateProductCategoryDto.IsActive.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            productCategory.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.ProductCategory.UpdateAsync(productCategory, cancellationToken);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
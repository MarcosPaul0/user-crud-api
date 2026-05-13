// <copyright file="UpdateProductUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.UpdateProduct;

public sealed class UpdateProductUseCase(IUnitOfWork unitOfWork) : IUpdateProductUseCase
{
    public async Task ExecuteAsync(Guid productId, UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Product.FindByIdAsync(productId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_NOT_FOUND);
        }

        var isUpdated = false;

        if (updateProductDto.Name != null && updateProductDto.Name != product.Name)
        {
            var productAlreadyExists = await unitOfWork.Product.FindByNameAsync(updateProductDto.Name, cancellationToken);

            if (productAlreadyExists != null)
            {
                throw new ConflictException(ExceptionMessages.PRODUCT_ALREADY_EXISTS);
            }

            product.Name = updateProductDto.Name;
            isUpdated = true;
        }

        if (updateProductDto.ProductCategoryId != null && updateProductDto.ProductCategoryId != product.ProductCategoryId)
        {
            var productCategory =
                await unitOfWork.ProductCategory.FindByIdAsync(updateProductDto.ProductCategoryId.Value, cancellationToken);

            if (productCategory == null)
            {
                throw new NotFoundException(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND);
            }

            product.ProductCategoryId = updateProductDto.ProductCategoryId.Value;
            isUpdated = true;
        }

        if (updateProductDto.PrintDescription != null && updateProductDto.PrintDescription != product.PrintDescription)
        {
            product.PrintDescription = updateProductDto.PrintDescription;
            isUpdated = true;
        }

        if (updateProductDto.Description != null && updateProductDto.Description != product.Description)
        {
            product.Description = updateProductDto.Description;
            isUpdated = true;
        }

        if (updateProductDto.PriceInCents != null && updateProductDto.PriceInCents != product.PriceInCents)
        {
            product.PriceInCents = updateProductDto.PriceInCents.Value;
            isUpdated = true;
        }

        if (updateProductDto.ProductionTimeInMinutes != null && updateProductDto.ProductionTimeInMinutes != product.ProductionTimeInMinutes)
        {
            product.ProductionTimeInMinutes = updateProductDto.ProductionTimeInMinutes.Value;
            isUpdated = true;
        }

        if (updateProductDto.DiscountPercentage != null && updateProductDto.DiscountPercentage != product.DiscountPercentage)
        {
            product.DiscountPercentage = updateProductDto.DiscountPercentage.Value;
            isUpdated = true;
        }

        if (updateProductDto.StockQuantity != null && updateProductDto.StockQuantity != product.StockQuantity)
        {
            product.StockQuantity = updateProductDto.StockQuantity.Value;
            isUpdated = true;
        }

        if (updateProductDto.IsActive != null && updateProductDto.IsActive != product.IsActive)
        {
            product.IsActive = updateProductDto.IsActive.Value;
            isUpdated = true;
        }

        if (updateProductDto.HeightInCentimeters != null && updateProductDto.HeightInCentimeters != product.HeightInCentimeters)
        {
            product.HeightInCentimeters = updateProductDto.HeightInCentimeters.Value;
            isUpdated = true;
        }

        if (updateProductDto.WidthInCentimeters != null && updateProductDto.WidthInCentimeters != product.WidthInCentimeters)
        {
            product.WidthInCentimeters = updateProductDto.WidthInCentimeters.Value;
            isUpdated = true;
        }

        if (updateProductDto.DepthInCentimeters != null && updateProductDto.DepthInCentimeters != product.DepthInCentimeters)
        {
            product.DepthInCentimeters = updateProductDto.DepthInCentimeters.Value;
            isUpdated = true;
        }

        if (updateProductDto.WeightInGrams != null && updateProductDto.WeightInGrams != product.WeightInGrams)
        {
            product.WeightInGrams = updateProductDto.WeightInGrams.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            product.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.Product.UpdateAsync(product, cancellationToken);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
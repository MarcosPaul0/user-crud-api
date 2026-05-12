// <copyright file="SetProductImagesUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace AutoriaStore.Application.UseCases.SetProductImages;

public sealed class SetProductImagesUseCase(
    IObjectStorageService objectStorageService,
    IUnitOfWork unitOfWork) : ISetProductImagesUseCase
{
    public async Task ExecuteAsync(
        Guid productId,
        SetProductImagesDto setProductImagesDto,
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Product.FindByIdAsync(productId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCTNOTFOUND);
        }

        var productImagesToCreate = new List<(IFormFile, ProductImage)>();
        var productImagesToUpdate = new List<(IFormFile?, ProductImage)>();

        var index = 0;
        foreach (var productImageDto in setProductImagesDto.Images)
        {
            if (productImageDto.Id != null && productImageDto.Id != Guid.Empty)
            {
                continue;
            }

            var productImage = new ProductImage(null!, productImageDto.DisplayOrder, productId, DateTime.UtcNow);

            productImagesToCreate.Add((productImageDto.File!, productImage));
            index++;
        }

        var productImages = await unitOfWork.ProductImage.FindAllByProductIdAsync(productId, cancellationToken);

        if (productImages.Count + productImagesToCreate.Count > 5)
        {
            throw new ConflictException(ExceptionMessages.PRODUCTMAXIMAGESREACHED);
        }

        foreach (var productImageDto in setProductImagesDto.Images)
        {
            if (productImageDto.Id == null || productImageDto.Id == Guid.Empty)
            {
                continue;
            }

            var productImage = await unitOfWork.ProductImage.FindByIdAsync(productImageDto.Id.Value, cancellationToken);

            if (productImage == null)
            {
                throw new NotFoundException(ExceptionMessages.PRODUCTIMAGENOTFOUND);
            }

            productImage.DisplayOrder = productImageDto.DisplayOrder;
            productImage.UpdatedAt = DateTime.UtcNow;

            productImagesToUpdate.Add((productImageDto.File, productImage));
        }

        foreach (var (file, productImage) in productImagesToCreate)
        {
            await this.CreateProductImagesAsync(productId, file, productImage, cancellationToken);
        }

        foreach (var (file, productImage) in productImagesToUpdate)
        {
            await this.UpdateProductImagesAsync(productImage, file, cancellationToken);
        }

        await unitOfWork.SaveChangesAsync();
    }

    private async Task CreateProductImagesAsync(
        Guid productId,
        IFormFile file,
        ProductImage productImage,
        CancellationToken cancellationToken)
    {
        if (file == null)
        {
            throw new ConflictException(ExceptionMessages.PRODUCTIMAGEFILEISREQUIRED);
        }

        var imageId = Guid.NewGuid();
        var extension = Path.GetExtension(file.FileName);
        var objectKey = $"products/{productId}/{imageId}{extension}";

        var imageUrl = await objectStorageService.UploadAsync(
            file,
            objectKey,
            cancellationToken);

        productImage.ImageUrl = imageUrl;

        await unitOfWork.ProductImage.CreateAsync(productImage, cancellationToken);
    }

    private async Task UpdateProductImagesAsync(
        ProductImage productImage,
        IFormFile? file,
        CancellationToken cancellationToken)
    {
        if (file != null)
        {
            await objectStorageService.DeleteAsync(productImage.ImageUrl, cancellationToken);

            var extension = Path.GetExtension(file.FileName);
            var objectKey = $"products/{productImage.ProductId}/{productImage.Id}{extension}";

            var newUrl = await objectStorageService.UploadAsync(
                file,
                objectKey,
                cancellationToken);

            productImage.ImageUrl = newUrl;
        }

        await unitOfWork.ProductImage.UpdateAsync(productImage, cancellationToken);
    }
}
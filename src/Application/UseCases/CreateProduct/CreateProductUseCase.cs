// <copyright file="CreateProductUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.CreateProduct;

public sealed class CreateProductUseCase(IUnitOfWork unitOfWork) : ICreateProductUseCase
{
    public async Task ExecuteAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
    {
        var productCategory =
            await unitOfWork.ProductCategory.FindByIdAsync(createProductDto.ProductCategoryId, cancellationToken);

        if (productCategory == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCTCATEGORYNOTFOUND);
        }

        var productAlreadyExists = await unitOfWork.Product.FindByNameAsync(createProductDto.Name, cancellationToken);

        if (productAlreadyExists != null)
        {
            throw new ConflictException(ExceptionMessages.PRODUCTALREADYEXISTS);
        }

        var newProduct = new Product()
        {
            Id = Guid.NewGuid(),
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            PrintDescription = createProductDto.PrintDescription,
            PriceInCents = createProductDto.PriceInCents,
            ProductionTimeInMinutes = createProductDto.ProductionTimeInMinutes,
            DiscountPercentage = createProductDto.DiscountPercentage,
            StockQuantity = createProductDto.StockQuantity,
            ProductCategoryId = createProductDto.ProductCategoryId,
            DepthInCentimeters = createProductDto.DepthInCentimeters,
            WidthInCentimeters = createProductDto.WidthInCentimeters,
            HeightInCentimeters = createProductDto.HeightInCentimeters,
            WeightInGrams = createProductDto.WeightInGrams,
            CreatedAt = DateTime.UtcNow,
        };

        await unitOfWork.Product.CreateAsync(newProduct, cancellationToken);
        await unitOfWork.SaveChangesAsync();
    }
}
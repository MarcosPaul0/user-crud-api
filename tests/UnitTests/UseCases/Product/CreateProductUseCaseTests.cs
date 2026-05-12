// <copyright file="CreateProductUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.CreateProduct;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class CreateProductUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductRepository> productRepositoryMock;
    private readonly Mock<IProductCategoryRepository> productCategoryRepositoryMock;
    private readonly CreateProductUseCase sut;

    public CreateProductUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productRepositoryMock = new Mock<IProductRepository>();
        this.productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        this.unitOfWorkMock.Setup(u => u.Product).Returns(this.productRepositoryMock.Object);
        this.unitOfWorkMock.Setup(u => u.ProductCategory).Returns(this.productCategoryRepositoryMock.Object);

        this.sut = new CreateProductUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductCategoryNotFound_ThrowsNotFoundException()
    {
        var dto = new CreateProductDto
        {
            Name = "Test Product Name",
            Description = "Test description for the product",
            PrintDescription = "Print description text",
            PriceInCents = 1000,
            ProductionTimeInMinutes = 30,
            DiscountPercentage = 0,
            StockQuantity = 10,
            ProductCategoryId = Guid.NewGuid(),
        };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(dto.ProductCategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.ProductCategory?)null);

        var act = () => this.sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCTCATEGORYNOTFOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductAlreadyExists_ThrowsConflictException()
    {
        var categoryId = Guid.NewGuid();
        var dto = new CreateProductDto
        {
            Name = "Test Product Name",
            Description = "Test description for the product",
            PrintDescription = "Print description text",
            PriceInCents = 1000,
            ProductionTimeInMinutes = 30,
            DiscountPercentage = 0,
            StockQuantity = 10,
            ProductCategoryId = categoryId,
        };

        var existingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = categoryId,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            CreatedAt = DateTime.UtcNow,
        };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        this.productRepositoryMock
            .Setup(r => r.FindByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        var act = () => this.sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.PRODUCTALREADYEXISTS, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenValidRequest_CreatesProductAndSavesChanges()
    {
        var categoryId = Guid.NewGuid();
        var dto = new CreateProductDto
        {
            Name = "Test Product Name",
            Description = "Test description for the product",
            PrintDescription = "Print description text",
            PriceInCents = 1000,
            ProductionTimeInMinutes = 30,
            DiscountPercentage = 5,
            StockQuantity = 10,
            ProductCategoryId = categoryId,
        };

        var existingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = categoryId,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        this.productRepositoryMock
            .Setup(r => r.FindByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.Product?)null);

        await this.sut.ExecuteAsync(dto, CancellationToken.None);

        this.productRepositoryMock.Verify(
            r => r.CreateAsync(
            It.Is<AutoriaStore.Domain.Entities.Product>(p =>
                p.Name == dto.Name &&
                p.Description == dto.Description &&
                p.PrintDescription == dto.PrintDescription &&
                p.PriceInCents == dto.PriceInCents &&
                p.ProductionTimeInMinutes == dto.ProductionTimeInMinutes &&
                p.DiscountPercentage == dto.DiscountPercentage &&
                p.StockQuantity == dto.StockQuantity &&
                p.ProductCategoryId == dto.ProductCategoryId),
            It.IsAny<CancellationToken>()), Times.Once);

        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

// <copyright file="UpdateProductUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.UpdateProduct;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class UpdateProductUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductRepository> productRepositoryMock;
    private readonly Mock<IProductCategoryRepository> productCategoryRepositoryMock;
    private readonly UpdateProductUseCase sut;

    public UpdateProductUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productRepositoryMock = new Mock<IProductRepository>();
        this.productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        this.unitOfWorkMock.Setup(u => u.Product).Returns(this.productRepositoryMock.Object);
        this.unitOfWorkMock.Setup(u => u.ProductCategory).Returns(this.productCategoryRepositoryMock.Object);

        this.sut = new UpdateProductUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        var dto = new UpdateProductDto { Name = "New Name" };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.Product?)null);

        var act = () => this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNewNameConflictsWithExistingProduct_ThrowsConflictException()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Original Product Name",
            CreatedAt = DateTime.UtcNow,
        };

        var conflictingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = "Conflicting Product",
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { Name = "Conflicting Product" };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productRepositoryMock
            .Setup(r => r.FindByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conflictingProduct);

        var act = () => this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_ALREADY_EXISTS, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNewCategoryNotFound_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        var newCategoryId = Guid.NewGuid();

        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Original Product",
            ProductCategoryId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { ProductCategoryId = newCategoryId };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(newCategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.ProductCategory?)null);

        var act = () => this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNothingChanged_DoesNotUpdateOrSave()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            Description = "Test description",
            PriceInCents = 1000,
            StockQuantity = 5,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto();

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        this.productRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AutoriaStore.Domain.Entities.Product>(), It.IsAny<CancellationToken>()), Times.Never);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNameChanged_UpdatesProductAndSavesChanges()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Original Name",
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { Name = "Updated Product Name" };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productRepositoryMock
            .Setup(r => r.FindByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.Product?)null);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal("Updated Product Name", existingProduct.Name);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenIsActiveChanged_UpdatesProductAndSavesChanges()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { IsActive = true };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.True(existingProduct.IsActive);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductCategoryIdChanged_AndCategoryFound_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var newCategoryId = Guid.NewGuid();

        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            ProductCategoryId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
        };

        var newCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = newCategoryId,
            Category = "New Category",
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { ProductCategoryId = newCategoryId };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(newCategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(newCategory);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal(newCategoryId, existingProduct.ProductCategoryId);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPrintDescriptionChanged_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            PrintDescription = "Old print description text",
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { PrintDescription = "New updated print description" };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal("New updated print description", existingProduct.PrintDescription);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenDescriptionChanged_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            Description = "Old description text here",
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { Description = "New updated description text" };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal("New updated description text", existingProduct.Description);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPriceInCentsChanged_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            PriceInCents = 1000,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { PriceInCents = 2500 };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal(2500, existingProduct.PriceInCents);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductionTimeInMinutesChanged_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            ProductionTimeInMinutes = 30,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { ProductionTimeInMinutes = 60 };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal(60, existingProduct.ProductionTimeInMinutes);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenDiscountPercentageChanged_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            DiscountPercentage = 0,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { DiscountPercentage = 10 };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal(10, existingProduct.DiscountPercentage);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenStockQuantityChanged_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            StockQuantity = 5,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { StockQuantity = 20 };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal(20, existingProduct.StockQuantity);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenHeightInCentimetersChanged_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            HeightInCentimeters = 10,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { HeightInCentimeters = 25 };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal(25, existingProduct.HeightInCentimeters);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenWidthInCentimetersChanged_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            WidthInCentimeters = 10,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { WidthInCentimeters = 30 };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal(30, existingProduct.WidthInCentimeters);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenDepthInCentimetersChanged_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            DepthInCentimeters = 5,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { DepthInCentimeters = 15 };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal(15, existingProduct.DepthInCentimeters);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenWeightInGramsChanged_UpdatesProductAndSaves()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            WeightInGrams = 500,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductDto { WeightInGrams = 1200 };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal(1200, existingProduct.WeightInGrams);
        this.productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

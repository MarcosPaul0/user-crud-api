// <copyright file="DeleteProductUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.DeleteProduct;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class DeleteProductUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductRepository> productRepositoryMock;
    private readonly Mock<IProductImageRepository> productImageRepositoryMock;
    private readonly Mock<IObjectStorageService> objectStorageMock;
    private readonly DeleteProductUseCase sut;

    public DeleteProductUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productRepositoryMock = new Mock<IProductRepository>();
        this.productImageRepositoryMock = new Mock<IProductImageRepository>();
        this.objectStorageMock = new Mock<IObjectStorageService>();

        this.unitOfWorkMock.Setup(u => u.Product).Returns(this.productRepositoryMock.Object);
        this.unitOfWorkMock.Setup(u => u.ProductImage).Returns(this.productImageRepositoryMock.Object);

        this.sut = new DeleteProductUseCase(this.objectStorageMock.Object, this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.Product?)null);

        var act = () => this.sut.ExecuteAsync(productId, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCTNOTFOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductHasImages_DeletesImagesFromStorageBeforeDeletingProduct()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow,
        };

        var productImages = new List<ProductImage>
        {
            new ("https://storage.example.com/image1.jpg", 1, productId, DateTime.UtcNow) { Id = Guid.NewGuid() },
            new ("https://storage.example.com/image2.jpg", 2, productId, DateTime.UtcNow) { Id = Guid.NewGuid() },
        };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productImageRepositoryMock
            .Setup(r => r.FindAllByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productImages);

        this.productRepositoryMock
            .Setup(r => r.DeleteAsync(existingProduct, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, CancellationToken.None);

        this.objectStorageMock.Verify(s => s.DeleteAsync("https://storage.example.com/image1.jpg", It.IsAny<CancellationToken>()), Times.Once);
        this.objectStorageMock.Verify(s => s.DeleteAsync("https://storage.example.com/image2.jpg", It.IsAny<CancellationToken>()), Times.Once);
        this.productRepositoryMock.Verify(r => r.DeleteAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductHasNoImages_DeletesProductDirectly()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow,
        };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productImageRepositoryMock
            .Setup(r => r.FindAllByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductImage>());

        this.productRepositoryMock
            .Setup(r => r.DeleteAsync(existingProduct, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await this.sut.ExecuteAsync(productId, CancellationToken.None);

        this.objectStorageMock.Verify(s => s.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        this.productRepositoryMock.Verify(r => r.DeleteAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

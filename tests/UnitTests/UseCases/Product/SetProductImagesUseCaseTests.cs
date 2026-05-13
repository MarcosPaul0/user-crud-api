// <copyright file="SetProductImagesUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.SetProductImages;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class SetProductImagesUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductRepository> productRepositoryMock;
    private readonly Mock<IProductImageRepository> productImageRepositoryMock;
    private readonly Mock<IObjectStorageService> objectStorageMock;
    private readonly SetProductImagesUseCase sut;

    public SetProductImagesUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productRepositoryMock = new Mock<IProductRepository>();
        this.productImageRepositoryMock = new Mock<IProductImageRepository>();
        this.objectStorageMock = new Mock<IObjectStorageService>();

        this.unitOfWorkMock.Setup(u => u.Product).Returns(this.productRepositoryMock.Object);
        this.unitOfWorkMock.Setup(u => u.ProductImage).Returns(this.productImageRepositoryMock.Object);

        this.sut = new SetProductImagesUseCase(this.objectStorageMock.Object, this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        var dto = new SetProductImagesDto { Images = new List<ProductImageDto>() };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.Product?)null);

        var act = () => this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenMaxImagesWouldBeExceeded_ThrowsConflictException()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow,
        };

        var existingImages = new List<ProductImage>
        {
            new ("url1", 1, productId, DateTime.UtcNow) { Id = Guid.NewGuid() },
            new ("url2", 2, productId, DateTime.UtcNow) { Id = Guid.NewGuid() },
            new ("url3", 3, productId, DateTime.UtcNow) { Id = Guid.NewGuid() },
            new ("url4", 4, productId, DateTime.UtcNow) { Id = Guid.NewGuid() },
        };

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("new_image.jpg");

        // 4 existing + 2 new = 6, exceeds limit of 5
        var dto = new SetProductImagesDto
        {
            Images = new List<ProductImageDto>
            {
                new () { File = mockFile.Object, DisplayOrder = 1 },
                new () { File = mockFile.Object, DisplayOrder = 2 },
            },
        };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productImageRepositoryMock
            .Setup(r => r.FindAllByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingImages);

        var act = () => this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_MAX_IMAGES_REACHED, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCreatingNewImages_UploadsFilesAndCreatesImageRecords()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow,
        };

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("photo.jpg");

        var dto = new SetProductImagesDto
        {
            Images = new List<ProductImageDto>
            {
                new () { File = mockFile.Object, DisplayOrder = 1 },
            },
        };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productImageRepositoryMock
            .Setup(r => r.FindAllByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductImage>());

        this.objectStorageMock
            .Setup(s => s.UploadAsync(mockFile.Object, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/products/image.jpg");

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        this.objectStorageMock.Verify(s => s.UploadAsync(mockFile.Object, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        this.productImageRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<ProductImage>(), It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUpdatingExistingImageWithNewFile_DeletesOldFileAndUploadsNew()
    {
        var productId = Guid.NewGuid();
        var imageId = Guid.NewGuid();

        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow,
        };

        var existingImage = new ProductImage("https://storage.example.com/old_image.jpg", 1, productId, DateTime.UtcNow)
        {
            Id = imageId,
        };

        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.FileName).Returns("new_photo.jpg");

        var dto = new SetProductImagesDto
        {
            Images = new List<ProductImageDto>
            {
                new () { Id = imageId, File = mockFile.Object, DisplayOrder = 2 },
            },
        };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productImageRepositoryMock
            .Setup(r => r.FindAllByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductImage>());

        this.productImageRepositoryMock
            .Setup(r => r.FindByIdAsync(imageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingImage);

        this.objectStorageMock
            .Setup(s => s.UploadAsync(mockFile.Object, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("https://storage.example.com/new_image.jpg");

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        this.objectStorageMock.Verify(s => s.DeleteAsync("https://storage.example.com/old_image.jpg", It.IsAny<CancellationToken>()), Times.Once);
        this.objectStorageMock.Verify(s => s.UploadAsync(mockFile.Object, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        this.productImageRepositoryMock.Verify(r => r.UpdateAsync(existingImage, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUpdatingExistingImageWithoutNewFile_OnlyUpdatesDisplayOrder()
    {
        var productId = Guid.NewGuid();
        var imageId = Guid.NewGuid();

        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow,
        };

        var existingImage = new ProductImage("https://storage.example.com/image.jpg", 1, productId, DateTime.UtcNow)
        {
            Id = imageId,
        };

        var dto = new SetProductImagesDto
        {
            Images = new List<ProductImageDto>
            {
                new () { Id = imageId, File = null, DisplayOrder = 3 },
            },
        };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productImageRepositoryMock
            .Setup(r => r.FindAllByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductImage>());

        this.productImageRepositoryMock
            .Setup(r => r.FindByIdAsync(imageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingImage);

        await this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        this.objectStorageMock.Verify(s => s.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        this.objectStorageMock.Verify(s => s.UploadAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.Equal(3, existingImage.DisplayOrder);
        this.productImageRepositoryMock.Verify(r => r.UpdateAsync(existingImage, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUpdatingImageNotFoundInRepository_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        var nonExistentImageId = Guid.NewGuid();

        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new SetProductImagesDto
        {
            Images = new List<ProductImageDto>
            {
                new () { Id = nonExistentImageId, File = null, DisplayOrder = 1 },
            },
        };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productImageRepositoryMock
            .Setup(r => r.FindAllByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductImage>());

        this.productImageRepositoryMock
            .Setup(r => r.FindByIdAsync(nonExistentImageId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductImage?)null);

        var act = () => this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_IMAGE_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCreatingNewImageWithNullFile_ThrowsConflictException()
    {
        var productId = Guid.NewGuid();

        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new SetProductImagesDto
        {
            Images = new List<ProductImageDto>
            {
                new () { File = null, DisplayOrder = 1 },
            },
        };

        this.productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        this.productImageRepositoryMock
            .Setup(r => r.FindAllByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductImage>());

        var act = () => this.sut.ExecuteAsync(productId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_IMAGE_FILE_IS_REQUIRED, exception.Message);
    }
}

// <copyright file="ListProductsUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.ListProducts;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class ListProductsUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductRepository> productRepositoryMock;
    private readonly ListProductsUseCase sut;

    public ListProductsUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productRepositoryMock = new Mock<IProductRepository>();

        this.unitOfWorkMock.Setup(u => u.Product).Returns(this.productRepositoryMock.Object);

        this.sut = new ListProductsUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_AlwaysFiltersActiveProductsAndActiveCategories()
    {
        var dto = new ListProductsDto { Page = 1, ItemsPerPage = 10 };

        this.productRepositoryMock
            .Setup(r => r.FindAllAsync(
                It.Is<AutoriaStore.Domain.Entities.Product>(f =>
                    f.IsActive == true &&
                    f.ProductCategory != null &&
                    f.ProductCategory.IsActive),
                dto.Page,
                dto.ItemsPerPage,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AutoriaStore.Domain.Entities.Product>());

        this.productRepositoryMock
            .Setup(r => r.CountAsync(
                It.Is<AutoriaStore.Domain.Entities.Product>(f =>
                    f.IsActive == true),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        await this.sut.ExecuteAsync(dto, CancellationToken.None);

        this.productRepositoryMock.Verify(
            r => r.FindAllAsync(
            It.Is<AutoriaStore.Domain.Entities.Product>(f =>
                f.IsActive == true &&
                f.ProductCategory != null &&
                f.ProductCategory.IsActive),
            dto.Page,
            dto.ItemsPerPage,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNameFilterProvided_AppliesNameToFilter()
    {
        var dto = new ListProductsDto { Page = 1, ItemsPerPage = 10, Name = "Laptop" };

        this.productRepositoryMock
            .Setup(r => r.FindAllAsync(
                It.Is<AutoriaStore.Domain.Entities.Product>(f => f.Name == "Laptop"),
                dto.Page,
                dto.ItemsPerPage,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AutoriaStore.Domain.Entities.Product>());

        this.productRepositoryMock
            .Setup(r => r.CountAsync(It.IsAny<AutoriaStore.Domain.Entities.Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        await this.sut.ExecuteAsync(dto, CancellationToken.None);

        this.productRepositoryMock.Verify(
            r => r.FindAllAsync(
            It.Is<AutoriaStore.Domain.Entities.Product>(f => f.Name == "Laptop"),
            dto.Page,
            dto.ItemsPerPage,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsProductsAndCount()
    {
        var dto = new ListProductsDto { Page = 1, ItemsPerPage = 10 };

        var products = new List<AutoriaStore.Domain.Entities.Product>
        {
            new () { Id = Guid.NewGuid(), Name = "Product A", CreatedAt = DateTime.UtcNow },
            new () { Id = Guid.NewGuid(), Name = "Product B", CreatedAt = DateTime.UtcNow },
        };

        this.productRepositoryMock
            .Setup(r => r.FindAllAsync(It.IsAny<AutoriaStore.Domain.Entities.Product>(), dto.Page, dto.ItemsPerPage, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        this.productRepositoryMock
            .Setup(r => r.CountAsync(It.IsAny<AutoriaStore.Domain.Entities.Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        var (resultProducts, resultCount) = await this.sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Equal(products, resultProducts);
        Assert.Equal(2, resultCount);
    }
}

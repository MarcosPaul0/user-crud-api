// <copyright file="ListProductsForAdminUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.ListProductsForAdmin;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class ListProductsForAdminUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductRepository> productRepositoryMock;
    private readonly ListProductsForAdminUseCase sut;

    public ListProductsForAdminUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productRepositoryMock = new Mock<IProductRepository>();

        this.unitOfWorkMock.Setup(u => u.Product).Returns(this.productRepositoryMock.Object);

        this.sut = new ListProductsForAdminUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoFilters_DoesNotApplyIsActiveFilter()
    {
        var dto = new ListProductsByAdminDto { Page = 1, ItemsPerPage = 10 };

        this.productRepositoryMock
            .Setup(r => r.FindAllAsync(
                It.Is<AutoriaStore.Domain.Entities.Product>(f => f.IsActive == null),
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
            It.Is<AutoriaStore.Domain.Entities.Product>(f => f.IsActive == null),
            dto.Page,
            dto.ItemsPerPage,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenIsActiveFilterProvided_AppliesIsActiveFilter()
    {
        var dto = new ListProductsByAdminDto { Page = 1, ItemsPerPage = 10, IsActive = false };

        this.productRepositoryMock
            .Setup(r => r.FindAllAsync(
                It.Is<AutoriaStore.Domain.Entities.Product>(f => f.IsActive == false),
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
            It.Is<AutoriaStore.Domain.Entities.Product>(f => f.IsActive == false),
            dto.Page,
            dto.ItemsPerPage,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsProductsAndCount()
    {
        var dto = new ListProductsByAdminDto { Page = 1, ItemsPerPage = 10 };

        var products = new List<AutoriaStore.Domain.Entities.Product>
        {
            new () { Id = Guid.NewGuid(), Name = "Active Product", IsActive = true, CreatedAt = DateTime.UtcNow },
            new () { Id = Guid.NewGuid(), Name = "Inactive Product", IsActive = false, CreatedAt = DateTime.UtcNow },
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

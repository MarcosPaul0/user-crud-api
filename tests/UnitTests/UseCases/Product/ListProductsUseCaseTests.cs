using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.ListProducts;
using AutoriaStore.Domain.Interfaces;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class ListProductsUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ListProductsUseCase _sut;

    public ListProductsUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _unitOfWorkMock.Setup(u => u.Product).Returns(_productRepositoryMock.Object);

        _sut = new ListProductsUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_AlwaysFiltersActiveProductsAndActiveCategories()
    {
        var dto = new ListProductsDto { Page = 1, ItemsPerPage = 10 };

        _productRepositoryMock
            .Setup(r => r.FindAllAsync(
                It.Is<AutoriaStore.Domain.Entities.Product>(f =>
                    f.IsActive == true &&
                    f.ProductCategory != null &&
                    f.ProductCategory.IsActive == true),
                dto.Page,
                dto.ItemsPerPage,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AutoriaStore.Domain.Entities.Product>());

        _productRepositoryMock
            .Setup(r => r.CountAsync(
                It.Is<AutoriaStore.Domain.Entities.Product>(f =>
                    f.IsActive == true),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        await _sut.ExecuteAsync(dto, CancellationToken.None);

        _productRepositoryMock.Verify(r => r.FindAllAsync(
            It.Is<AutoriaStore.Domain.Entities.Product>(f =>
                f.IsActive == true &&
                f.ProductCategory != null &&
                f.ProductCategory.IsActive == true),
            dto.Page,
            dto.ItemsPerPage,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNameFilterProvided_AppliesNameToFilter()
    {
        var dto = new ListProductsDto { Page = 1, ItemsPerPage = 10, Name = "Laptop" };

        _productRepositoryMock
            .Setup(r => r.FindAllAsync(
                It.Is<AutoriaStore.Domain.Entities.Product>(f => f.Name == "Laptop"),
                dto.Page,
                dto.ItemsPerPage,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AutoriaStore.Domain.Entities.Product>());

        _productRepositoryMock
            .Setup(r => r.CountAsync(It.IsAny<AutoriaStore.Domain.Entities.Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        await _sut.ExecuteAsync(dto, CancellationToken.None);

        _productRepositoryMock.Verify(r => r.FindAllAsync(
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
            new() { Id = Guid.NewGuid(), Name = "Product A", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Product B", CreatedAt = DateTime.UtcNow }
        };

        _productRepositoryMock
            .Setup(r => r.FindAllAsync(It.IsAny<AutoriaStore.Domain.Entities.Product>(), dto.Page, dto.ItemsPerPage, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        _productRepositoryMock
            .Setup(r => r.CountAsync(It.IsAny<AutoriaStore.Domain.Entities.Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        var (resultProducts, resultCount) = await _sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Equal(products, resultProducts);
        Assert.Equal(2, resultCount);
    }
}

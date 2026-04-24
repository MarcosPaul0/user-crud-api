using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.ListProductsForAdmin;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class ListProductsForAdminUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ListProductsForAdminUseCase _sut;

    public ListProductsForAdminUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _unitOfWorkMock.Setup(u => u.Product).Returns(_productRepositoryMock.Object);

        _sut = new ListProductsForAdminUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoFilters_DoesNotApplyIsActiveFilter()
    {
        var dto = new ListProductsByAdminDto { Page = 1, ItemsPerPage = 10 };

        _productRepositoryMock
            .Setup(r => r.FindAllAsync(
                It.Is<AutoriaStore.Domain.Entities.Product>(f => f.IsActive == null),
                dto.Page,
                dto.ItemsPerPage,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AutoriaStore.Domain.Entities.Product>());

        _productRepositoryMock
            .Setup(r => r.CountAsync(It.IsAny<AutoriaStore.Domain.Entities.Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        await _sut.ExecuteAsync(dto, CancellationToken.None);

        _productRepositoryMock.Verify(r => r.FindAllAsync(
            It.Is<AutoriaStore.Domain.Entities.Product>(f => f.IsActive == null),
            dto.Page,
            dto.ItemsPerPage,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenIsActiveFilterProvided_AppliesIsActiveFilter()
    {
        var dto = new ListProductsByAdminDto { Page = 1, ItemsPerPage = 10, IsActive = false };

        _productRepositoryMock
            .Setup(r => r.FindAllAsync(
                It.Is<AutoriaStore.Domain.Entities.Product>(f => f.IsActive == false),
                dto.Page,
                dto.ItemsPerPage,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AutoriaStore.Domain.Entities.Product>());

        _productRepositoryMock
            .Setup(r => r.CountAsync(It.IsAny<AutoriaStore.Domain.Entities.Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        await _sut.ExecuteAsync(dto, CancellationToken.None);

        _productRepositoryMock.Verify(r => r.FindAllAsync(
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
            new() { Id = Guid.NewGuid(), Name = "Active Product", IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Inactive Product", IsActive = false, CreatedAt = DateTime.UtcNow }
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

using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.FindProductById;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class FindProductByIdUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly FindProductByIdUseCase _sut;

    public FindProductByIdUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _unitOfWorkMock.Setup(u => u.Product).Returns(_productRepositoryMock.Object);

        _sut = new FindProductByIdUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.Product?)null);

        var act = () => _sut.ExecuteAsync(productId, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductFound_ReturnsProduct()
    {
        var productId = Guid.NewGuid();
        var expectedProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow
        };

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedProduct);

        var result = await _sut.ExecuteAsync(productId, CancellationToken.None);

        Assert.Equal(expectedProduct, result);
    }
}

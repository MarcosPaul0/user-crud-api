using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.DeleteProduct;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class DeleteProductUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IProductImageRepository> _productImageRepositoryMock;
    private readonly Mock<IObjectStorageService> _objectStorageMock;
    private readonly DeleteProductUseCase _sut;

    public DeleteProductUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _productImageRepositoryMock = new Mock<IProductImageRepository>();
        _objectStorageMock = new Mock<IObjectStorageService>();

        _unitOfWorkMock.Setup(u => u.Product).Returns(_productRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.ProductImage).Returns(_productImageRepositoryMock.Object);

        _sut = new DeleteProductUseCase(_objectStorageMock.Object, _unitOfWorkMock.Object);
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
    public async Task ExecuteAsync_WhenProductHasImages_DeletesImagesFromStorageBeforeDeletingProduct()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow
        };

        var productImages = new List<ProductImage>
        {
            new("https://storage.example.com/image1.jpg", 1, productId, DateTime.UtcNow) { Id = Guid.NewGuid() },
            new("https://storage.example.com/image2.jpg", 2, productId, DateTime.UtcNow) { Id = Guid.NewGuid() }
        };

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        _productImageRepositoryMock
            .Setup(r => r.FindAllByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(productImages);

        _productRepositoryMock
            .Setup(r => r.DeleteAsync(existingProduct, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await _sut.ExecuteAsync(productId, CancellationToken.None);

        _objectStorageMock.Verify(s => s.DeleteAsync("https://storage.example.com/image1.jpg", It.IsAny<CancellationToken>()), Times.Once);
        _objectStorageMock.Verify(s => s.DeleteAsync("https://storage.example.com/image2.jpg", It.IsAny<CancellationToken>()), Times.Once);
        _productRepositoryMock.Verify(r => r.DeleteAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductHasNoImages_DeletesProductDirectly()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Test Product",
            CreatedAt = DateTime.UtcNow
        };

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        _productImageRepositoryMock
            .Setup(r => r.FindAllByProductIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ProductImage>());

        _productRepositoryMock
            .Setup(r => r.DeleteAsync(existingProduct, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await _sut.ExecuteAsync(productId, CancellationToken.None);

        _objectStorageMock.Verify(s => s.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _productRepositoryMock.Verify(r => r.DeleteAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

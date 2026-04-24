using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.UpdateProduct;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class UpdateProductUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
    private readonly UpdateProductUseCase _sut;

    public UpdateProductUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        _unitOfWorkMock.Setup(u => u.Product).Returns(_productRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.ProductCategory).Returns(_productCategoryRepositoryMock.Object);

        _sut = new UpdateProductUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductNotFound_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        var dto = new UpdateProductDto { Name = "New Name" };

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.Product?)null);

        var act = () => _sut.ExecuteAsync(productId, dto, CancellationToken.None);

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
            CreatedAt = DateTime.UtcNow
        };

        var conflictingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = "Conflicting Product",
            CreatedAt = DateTime.UtcNow
        };

        var dto = new UpdateProductDto { Name = "Conflicting Product" };

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        _productRepositoryMock
            .Setup(r => r.FindByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conflictingProduct);

        var act = () => _sut.ExecuteAsync(productId, dto, CancellationToken.None);

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
            CreatedAt = DateTime.UtcNow
        };

        var dto = new UpdateProductDto { ProductCategoryId = newCategoryId };

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(newCategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.ProductCategory?)null);

        var act = () => _sut.ExecuteAsync(productId, dto, CancellationToken.None);

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
            CreatedAt = DateTime.UtcNow
        };

        var dto = new UpdateProductDto();

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await _sut.ExecuteAsync(productId, dto, CancellationToken.None);

        _productRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AutoriaStore.Domain.Entities.Product>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNameChanged_UpdatesProductAndSavesChanges()
    {
        var productId = Guid.NewGuid();
        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = productId,
            Name = "Original Name",
            CreatedAt = DateTime.UtcNow
        };

        var dto = new UpdateProductDto { Name = "Updated Product Name" };

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        _productRepositoryMock
            .Setup(r => r.FindByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.Product?)null);

        await _sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.Equal("Updated Product Name", existingProduct.Name);
        _productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
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
            CreatedAt = DateTime.UtcNow
        };

        var dto = new UpdateProductDto { IsActive = true };

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        await _sut.ExecuteAsync(productId, dto, CancellationToken.None);

        Assert.True(existingProduct.IsActive);
        _productRepositoryMock.Verify(r => r.UpdateAsync(existingProduct, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

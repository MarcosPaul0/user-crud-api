using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.CreateProduct;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.Product;

public class CreateProductUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
    private readonly CreateProductUseCase _sut;

    public CreateProductUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        _unitOfWorkMock.Setup(u => u.Product).Returns(_productRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.ProductCategory).Returns(_productCategoryRepositoryMock.Object);

        _sut = new CreateProductUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductCategoryNotFound_ThrowsNotFoundException()
    {
        var dto = new CreateProductDto
        {
            Name = "Test Product Name",
            Description = "Test description for the product",
            PrintDescription = "Print description text",
            PriceInCents = 1000,
            ProductionTimeInMinutes = 30,
            DiscountPercentage = 0,
            StockQuantity = 10,
            ProductCategoryId = Guid.NewGuid()
        };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(dto.ProductCategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.ProductCategory?)null);

        var act = () => _sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductAlreadyExists_ThrowsConflictException()
    {
        var categoryId = Guid.NewGuid();
        var dto = new CreateProductDto
        {
            Name = "Test Product Name",
            Description = "Test description for the product",
            PrintDescription = "Print description text",
            PriceInCents = 1000,
            ProductionTimeInMinutes = 30,
            DiscountPercentage = 0,
            StockQuantity = 10,
            ProductCategoryId = categoryId
        };

        var existingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = categoryId,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var existingProduct = new AutoriaStore.Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            CreatedAt = DateTime.UtcNow
        };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        _productRepositoryMock
            .Setup(r => r.FindByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        var act = () => _sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_ALREADY_EXISTS, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenValidRequest_CreatesProductAndSavesChanges()
    {
        var categoryId = Guid.NewGuid();
        var dto = new CreateProductDto
        {
            Name = "Test Product Name",
            Description = "Test description for the product",
            PrintDescription = "Print description text",
            PriceInCents = 1000,
            ProductionTimeInMinutes = 30,
            DiscountPercentage = 5,
            StockQuantity = 10,
            ProductCategoryId = categoryId
        };

        var existingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = categoryId,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        _productRepositoryMock
            .Setup(r => r.FindByNameAsync(dto.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.Product?)null);

        await _sut.ExecuteAsync(dto, CancellationToken.None);

        _productRepositoryMock.Verify(r => r.CreateAsync(
            It.Is<AutoriaStore.Domain.Entities.Product>(p =>
                p.Name == dto.Name &&
                p.Description == dto.Description &&
                p.PrintDescription == dto.PrintDescription &&
                p.PriceInCents == dto.PriceInCents &&
                p.ProductionTimeInMinutes == dto.ProductionTimeInMinutes &&
                p.DiscountPercentage == dto.DiscountPercentage &&
                p.StockQuantity == dto.StockQuantity &&
                p.ProductCategoryId == dto.ProductCategoryId),
            It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

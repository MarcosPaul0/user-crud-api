using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.CreateProductCategory;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class CreateProductCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
    private readonly CreateProductCategoryUseCase _sut;

    public CreateProductCategoryUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        _unitOfWorkMock.Setup(u => u.ProductCategory).Returns(_productCategoryRepositoryMock.Object);

        _sut = new CreateProductCategoryUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryAlreadyExists_ThrowsConflictException()
    {
        var dto = new CreateProductCategoryDto { Category = "Electronics" };

        var existingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = Guid.NewGuid(),
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByCategoryAsync(dto.Category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        var act = () => _sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_CATEGORY_ALREADY_EXISTS, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryDoesNotExist_CreatesCategoryAndSavesChanges()
    {
        var dto = new CreateProductCategoryDto { Category = "Electronics" };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByCategoryAsync(dto.Category, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.ProductCategory?)null);

        await _sut.ExecuteAsync(dto, CancellationToken.None);

        _productCategoryRepositoryMock.Verify(r => r.CreateAsync(
            It.Is<AutoriaStore.Domain.Entities.ProductCategory>(c =>
                c.Category == dto.Category &&
                c.IsActive == false),
            It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

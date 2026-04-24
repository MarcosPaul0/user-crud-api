using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.UpdateProductCategory;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class UpdateProductCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
    private readonly UpdateProductCategoryUseCase _sut;

    public UpdateProductCategoryUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        _unitOfWorkMock.Setup(u => u.ProductCategory).Returns(_productCategoryRepositoryMock.Object);

        _sut = new UpdateProductCategoryUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryNotFound_ThrowsNotFoundException()
    {
        var categoryId = Guid.NewGuid();
        var dto = new UpdateProductCategoryDto { Category = "New Name" };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.ProductCategory?)null);

        var act = () => _sut.ExecuteAsync(categoryId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryNameConflictsWithDifferentCategory_ThrowsConflictException()
    {
        var categoryId = Guid.NewGuid();
        var existingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = categoryId,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var conflictingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = Guid.NewGuid(),
            Category = "Books",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var dto = new UpdateProductCategoryDto { Category = "Books" };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        _productCategoryRepositoryMock
            .Setup(r => r.FindByCategoryAsync(dto.Category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conflictingCategory);

        var act = () => _sut.ExecuteAsync(categoryId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_CATEGORY_ALREADY_EXISTS, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryNameMatchesSameCategory_DoesNotThrow()
    {
        var categoryId = Guid.NewGuid();
        var existingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = categoryId,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var dto = new UpdateProductCategoryDto { Category = "Electronics" };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        _productCategoryRepositoryMock
            .Setup(r => r.FindByCategoryAsync(dto.Category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        await _sut.ExecuteAsync(categoryId, dto, CancellationToken.None);

        _productCategoryRepositoryMock.Verify(r => r.UpdateAsync(existingCategory, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNothingChanged_DoesNotUpdateOrSave()
    {
        var categoryId = Guid.NewGuid();
        var existingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = categoryId,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var dto = new UpdateProductCategoryDto { Category = null, IsActive = null };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        await _sut.ExecuteAsync(categoryId, dto, CancellationToken.None);

        _productCategoryRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AutoriaStore.Domain.Entities.ProductCategory>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenIsActiveChanged_UpdatesCategoryAndSavesChanges()
    {
        var categoryId = Guid.NewGuid();
        var existingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = categoryId,
            Category = "Electronics",
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        };

        var dto = new UpdateProductCategoryDto { IsActive = true };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        await _sut.ExecuteAsync(categoryId, dto, CancellationToken.None);

        Assert.True(existingCategory.IsActive);
        _productCategoryRepositoryMock.Verify(r => r.UpdateAsync(existingCategory, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

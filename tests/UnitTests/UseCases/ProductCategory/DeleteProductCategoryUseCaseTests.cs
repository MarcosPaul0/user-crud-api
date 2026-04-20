using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.DeleteProductCategory;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using Moq;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class DeleteProductCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
    private readonly DeleteProductCategoryUseCase _sut;

    public DeleteProductCategoryUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        _unitOfWorkMock.Setup(u => u.ProductCategory).Returns(_productCategoryRepositoryMock.Object);

        _sut = new DeleteProductCategoryUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryNotFound_ThrowsNotFoundException()
    {
        var categoryId = Guid.NewGuid();

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.ProductCategory?)null);

        var act = () => _sut.ExecuteAsync(categoryId, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryFound_DeletesCategoryAndSavesChanges()
    {
        var categoryId = Guid.NewGuid();
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

        _productCategoryRepositoryMock
            .Setup(r => r.DeleteAsync(existingCategory, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        await _sut.ExecuteAsync(categoryId, CancellationToken.None);

        _productCategoryRepositoryMock.Verify(r => r.DeleteAsync(existingCategory, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

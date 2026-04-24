using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.FindProductCategoryByIdForAdmin;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class FindProductCategoryByIdForAdminUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
    private readonly FindProductCategoryByIdForAdminUseCase _sut;

    public FindProductCategoryByIdForAdminUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        _unitOfWorkMock.Setup(u => u.ProductCategory).Returns(_productCategoryRepositoryMock.Object);

        _sut = new FindProductCategoryByIdForAdminUseCase(_unitOfWorkMock.Object);
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
    public async Task ExecuteAsync_WhenCategoryFound_ReturnsProductCategory()
    {
        var categoryId = Guid.NewGuid();
        var expectedCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = categoryId,
            Category = "Electronics",
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        };

        _productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedCategory);

        var result = await _sut.ExecuteAsync(categoryId, CancellationToken.None);

        Assert.Equal(expectedCategory, result);
    }
}

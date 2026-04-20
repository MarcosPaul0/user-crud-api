using AutoriaStore.Application.UseCases.ListProductCategory;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using Moq;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class ListProductCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
    private readonly ListProductCategoryUseCase _sut;

    public ListProductCategoryUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        _unitOfWorkMock.Setup(u => u.ProductCategory).Returns(_productCategoryRepositoryMock.Object);

        _sut = new ListProductCategoryUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_QueriesWithActiveFilter_ReturnsActiveCategoriesAndCount()
    {
        var activeCategories = new List<AutoriaStore.Domain.Entities.ProductCategory>
        {
            new() { Id = Guid.NewGuid(), Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Category = "Books", IsActive = true, CreatedAt = DateTime.UtcNow }
        };

        _productCategoryRepositoryMock
            .Setup(r => r.FindAllAsync(
                It.Is<AutoriaStore.Domain.Entities.ProductCategory>(f => f.IsActive == true),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(activeCategories);

        var (resultCategories, resultCount) = await _sut.ExecuteAsync(CancellationToken.None);

        Assert.Equal(activeCategories, resultCategories);
        Assert.Equal(activeCategories.Count, resultCount);
    }
}

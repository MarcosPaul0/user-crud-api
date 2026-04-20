using AutoriaStore.Application.UseCases.ListProductCategoryForAdmin;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using Moq;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class ListProductCategoryForAdminUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> _productCategoryRepositoryMock;
    private readonly ListProductCategoryForAdminUseCase _sut;

    public ListProductCategoryForAdminUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        _unitOfWorkMock.Setup(u => u.ProductCategory).Returns(_productCategoryRepositoryMock.Object);

        _sut = new ListProductCategoryForAdminUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_QueriesWithNullFilter_ReturnsAllCategoriesAndCount()
    {
        var allCategories = new List<AutoriaStore.Domain.Entities.ProductCategory>
        {
            new() { Id = Guid.NewGuid(), Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Category = "Discontinued", IsActive = false, CreatedAt = DateTime.UtcNow }
        };

        _productCategoryRepositoryMock
            .Setup(r => r.FindAllAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(allCategories);

        var (resultCategories, resultCount) = await _sut.ExecuteAsync(CancellationToken.None);

        Assert.Equal(allCategories, resultCategories);
        Assert.Equal(allCategories.Count, resultCount);
    }
}

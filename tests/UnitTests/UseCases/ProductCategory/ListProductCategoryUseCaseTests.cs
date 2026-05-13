// <copyright file="ListProductCategoryUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.UseCases.ListProductCategory;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class ListProductCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> productCategoryRepositoryMock;
    private readonly ListProductCategoryUseCase sut;

    public ListProductCategoryUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        this.unitOfWorkMock.Setup(u => u.ProductCategory).Returns(this.productCategoryRepositoryMock.Object);

        this.sut = new ListProductCategoryUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_QueriesWithActiveFilter_ReturnsActiveCategoriesAndCount()
    {
        var activeCategories = new List<AutoriaStore.Domain.Entities.ProductCategory>
        {
            new () { Id = Guid.NewGuid(), Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow },
            new () { Id = Guid.NewGuid(), Category = "Books", IsActive = true, CreatedAt = DateTime.UtcNow },
        };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindAllAsync(
                It.Is<AutoriaStore.Domain.Entities.ProductCategory>(f => f.IsActive),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(activeCategories);

        var (resultCategories, resultCount) = await this.sut.ExecuteAsync(CancellationToken.None);

        Assert.Equal(activeCategories, resultCategories);
        Assert.Equal(activeCategories.Count, resultCount);
    }
}

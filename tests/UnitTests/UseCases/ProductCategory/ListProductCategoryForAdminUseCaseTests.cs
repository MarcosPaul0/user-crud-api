// <copyright file="ListProductCategoryForAdminUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.UseCases.ListProductCategoryForAdmin;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class ListProductCategoryForAdminUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> productCategoryRepositoryMock;
    private readonly ListProductCategoryForAdminUseCase sut;

    public ListProductCategoryForAdminUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        this.unitOfWorkMock.Setup(u => u.ProductCategory).Returns(this.productCategoryRepositoryMock.Object);

        this.sut = new ListProductCategoryForAdminUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_QueriesWithNullFilter_ReturnsAllCategoriesAndCount()
    {
        var allCategories = new List<AutoriaStore.Domain.Entities.ProductCategory>
        {
            new () { Id = Guid.NewGuid(), Category = "Electronics", IsActive = true, CreatedAt = DateTime.UtcNow },
            new () { Id = Guid.NewGuid(), Category = "Discontinued", IsActive = false, CreatedAt = DateTime.UtcNow },
        };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindAllAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(allCategories);

        var (resultCategories, resultCount) = await this.sut.ExecuteAsync(CancellationToken.None);

        Assert.Equal(allCategories, resultCategories);
        Assert.Equal(allCategories.Count, resultCount);
    }
}

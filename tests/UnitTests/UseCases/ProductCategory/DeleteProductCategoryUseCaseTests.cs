// <copyright file="DeleteProductCategoryUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.DeleteProductCategory;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class DeleteProductCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> productCategoryRepositoryMock;
    private readonly DeleteProductCategoryUseCase sut;

    public DeleteProductCategoryUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        this.unitOfWorkMock.Setup(u => u.ProductCategory).Returns(this.productCategoryRepositoryMock.Object);

        this.sut = new DeleteProductCategoryUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryNotFound_ThrowsNotFoundException()
    {
        var categoryId = Guid.NewGuid();

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.ProductCategory?)null);

        var act = () => this.sut.ExecuteAsync(categoryId, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCTCATEGORYNOTFOUND, exception.Message);
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
            CreatedAt = DateTime.UtcNow,
        };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        this.productCategoryRepositoryMock
            .Setup(r => r.DeleteAsync(existingCategory, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        await this.sut.ExecuteAsync(categoryId, CancellationToken.None);

        this.productCategoryRepositoryMock.Verify(r => r.DeleteAsync(existingCategory, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

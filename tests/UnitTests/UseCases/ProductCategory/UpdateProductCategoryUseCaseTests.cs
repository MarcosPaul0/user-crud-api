// <copyright file="UpdateProductCategoryUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.UpdateProductCategory;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class UpdateProductCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> productCategoryRepositoryMock;
    private readonly UpdateProductCategoryUseCase sut;

    public UpdateProductCategoryUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        this.unitOfWorkMock.Setup(u => u.ProductCategory).Returns(this.productCategoryRepositoryMock.Object);

        this.sut = new UpdateProductCategoryUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryNotFound_ThrowsNotFoundException()
    {
        var categoryId = Guid.NewGuid();
        var dto = new UpdateProductCategoryDto { Category = "New Name" };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.ProductCategory?)null);

        var act = () => this.sut.ExecuteAsync(categoryId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.PRODUCTCATEGORYNOTFOUND, exception.Message);
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
            CreatedAt = DateTime.UtcNow,
        };

        var conflictingCategory = new AutoriaStore.Domain.Entities.ProductCategory
        {
            Id = Guid.NewGuid(),
            Category = "Books",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductCategoryDto { Category = "Books" };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByCategoryAsync(dto.Category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conflictingCategory);

        var act = () => this.sut.ExecuteAsync(categoryId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.PRODUCTCATEGORYALREADYEXISTS, exception.Message);
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
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductCategoryDto { Category = "Electronics" };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByCategoryAsync(dto.Category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        await this.sut.ExecuteAsync(categoryId, dto, CancellationToken.None);

        this.productCategoryRepositoryMock.Verify(r => r.UpdateAsync(existingCategory, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
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
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductCategoryDto { Category = null, IsActive = null };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        await this.sut.ExecuteAsync(categoryId, dto, CancellationToken.None);

        this.productCategoryRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AutoriaStore.Domain.Entities.ProductCategory>(), It.IsAny<CancellationToken>()), Times.Never);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
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
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new UpdateProductCategoryDto { IsActive = true };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        await this.sut.ExecuteAsync(categoryId, dto, CancellationToken.None);

        Assert.True(existingCategory.IsActive);
        this.productCategoryRepositoryMock.Verify(r => r.UpdateAsync(existingCategory, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

// <copyright file="CreateProductCategoryUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.CreateProductCategory;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.ProductCategory;

public class CreateProductCategoryUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductCategoryRepository> productCategoryRepositoryMock;
    private readonly CreateProductCategoryUseCase sut;

    public CreateProductCategoryUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productCategoryRepositoryMock = new Mock<IProductCategoryRepository>();

        this.unitOfWorkMock.Setup(u => u.ProductCategory).Returns(this.productCategoryRepositoryMock.Object);

        this.sut = new CreateProductCategoryUseCase(this.unitOfWorkMock.Object);
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
            CreatedAt = DateTime.UtcNow,
        };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByCategoryAsync(dto.Category, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCategory);

        var act = () => this.sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.PRODUCTCATEGORYALREADYEXISTS, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCategoryDoesNotExist_CreatesCategoryAndSavesChanges()
    {
        var dto = new CreateProductCategoryDto { Category = "Electronics" };

        this.productCategoryRepositoryMock
            .Setup(r => r.FindByCategoryAsync(dto.Category, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.ProductCategory?)null);

        await this.sut.ExecuteAsync(dto, CancellationToken.None);

        this.productCategoryRepositoryMock.Verify(
            r => r.CreateAsync(
            It.Is<AutoriaStore.Domain.Entities.ProductCategory>(c =>
                c.Category == dto.Category &&
!c.IsActive),
            It.IsAny<CancellationToken>()), Times.Once);

        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

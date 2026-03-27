using AutoriaStore.Application.Dtos;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using UserCrud.Application.Exceptions;

namespace AutoriaStore.Application.UseCases.CreateProductCategory;

public sealed class CreateProductCategoryUseCase(IUnitOfWork unitOfWork) : ICreateProductCategoryUseCase
{
    public async Task ExecuteAsync(CreateProductCategoryDto createProductCategoryDto, CancellationToken cancellationToken)
    {
        var productCategoryAlreadyExists =
            await unitOfWork.ProductCategory.FindByCategoryAsync(createProductCategoryDto.Category, cancellationToken);

        if (productCategoryAlreadyExists != null)
        {
            throw new ConflictException(ExceptionMessages.PRODUCT_CATEGORY_ALREADY_EXISTS);
        }

        var newProductCategory = new ProductCategory()
        {
            Id = Guid.NewGuid(),
            Category = createProductCategoryDto.Category,
            IsActive = false,
            CreatedAt = DateTime.UtcNow,
        };

        await unitOfWork.ProductCategory.CreateAsync(newProductCategory, cancellationToken);
        await unitOfWork.SaveChangesAsync();
    }
}
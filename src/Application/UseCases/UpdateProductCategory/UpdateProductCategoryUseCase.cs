using UserCrud.Application.Dtos;
using UserCrud.Application.Exceptions;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.UpdateProductCategory;

public sealed class UpdateProductCategoryUseCase(IUnitOfWork unitOfWork) : IUpdateProductCategoryUseCase
{
    public async Task ExecuteAsync(
        Guid productCategoryId,
        UpdateProductCategoryDto updateProductCategoryDto, 
        CancellationToken cancellationToken)
    {
        var productCategory =
            await unitOfWork.ProductCategory.FindByIdAsync(productCategoryId, cancellationToken);
        
        if (productCategory == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND);
        }
        
        var productCategoryAlreadyExists =
            await unitOfWork.ProductCategory.FindByCategoryAsync(updateProductCategoryDto.Category, cancellationToken);

        if (productCategoryAlreadyExists != null)
        {
            throw new ConflictException(ExceptionMessages.PRODUCT_CATEGORY_ALREADY_EXISTS);
        }

        productCategory.Category = updateProductCategoryDto.Category;
        productCategory.UpdatedAt = DateTime.UtcNow;

        await unitOfWork.ProductCategory.UpdateAsync(productCategory, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();
    }
}
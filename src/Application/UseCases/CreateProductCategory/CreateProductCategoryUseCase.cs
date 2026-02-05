using UserCrud.Application.Dtos;
using UserCrud.Application.Exceptions;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.CreateProductCategory;

public class CreateProductCategoryUseCase(
    IProductCategoryRepository productCategoryRepository,
    IUnitOfWork unitOfWork) : ICreateProductCategoryUseCase
{
    public async Task ExecuteAsync(CreateProductCategoryDto createProductCategoryDto, CancellationToken cancellationToken)
    {
        var productCategoryAlreadyExists =
            await productCategoryRepository.FindByCategoryAsync(createProductCategoryDto.Category, cancellationToken);

        if (productCategoryAlreadyExists != null)
        {
            throw new ConflictException(ExceptionMessages.PRODUCT_CATEGORY_ALREADY_EXISTS);
        }

        var newProductCategory = new ProductCategory(createProductCategoryDto.Category, DateTime.UtcNow);

        await productCategoryRepository.CreateAsync(newProductCategory, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();
    }
}
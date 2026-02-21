using UserCrud.Application.Exceptions;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.FindProductCategoryById;

public class FindProductCategoryByIdUseCase(IProductCategoryRepository productCategoryRepository) : IFindProductCategoryByIdUseCase
{
    public async Task<ProductCategory> ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken)
    {
        var productCategory = await productCategoryRepository.FindByIdAsync(productCategoryId, cancellationToken);

        if (productCategory == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND);
        }
        
        return productCategory;
    }
}
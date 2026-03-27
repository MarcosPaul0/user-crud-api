using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using UserCrud.Application.Exceptions;

namespace UserCrud.Application.UseCases.FindProductCategoryById;

public sealed class FindProductCategoryByIdUseCase(IUnitOfWork unitOfWork) : IFindProductCategoryByIdUseCase
{
    public async Task<ProductCategory> ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken)
    {
        var productCategory = await unitOfWork.ProductCategory.FindByIdAsync(productCategoryId, cancellationToken);

        if (productCategory == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND);
        }
        
        return productCategory;
    }
}
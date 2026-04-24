using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.FindProductCategoryByIdForAdmin;

public class FindProductCategoryByIdForAdminUseCase(IUnitOfWork unitOfWork) : IFindProductCategoryByIdForAdminUseCase
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
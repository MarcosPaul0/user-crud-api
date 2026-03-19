using UserCrud.Application.Exceptions;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.DeleteProductCategory;

public sealed class DeleteProductCategoryUseCase(IUnitOfWork unitOfWork) : IDeleteProductCategoryUseCase
{
    public async Task ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken)
    {
        var productCategory = await unitOfWork.ProductCategory.FindByIdAsync(productCategoryId, cancellationToken);

        if (productCategory == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND);
        }
        
        await unitOfWork.ProductCategory.DeleteAsync(productCategory, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();
    }
}
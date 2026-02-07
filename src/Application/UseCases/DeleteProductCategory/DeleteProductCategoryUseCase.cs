using UserCrud.Application.Exceptions;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.DeleteProductCategory;

public class DeleteProductCategoryUseCase(
    IProductCategoryRepository productCategoryRepository,
    IUnitOfWork unitOfWork) : IDeleteProductCategoryUseCase
{
    public async Task ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken)
    {
        var productCategory = await productCategoryRepository.FindByIdAsync(productCategoryId, cancellationToken);

        if (productCategory == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND);
        }
        
        await productCategoryRepository.DeleteAsync(productCategory, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();
    }
}
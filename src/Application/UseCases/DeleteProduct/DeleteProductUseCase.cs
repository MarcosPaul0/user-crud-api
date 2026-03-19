using UserCrud.Application.Exceptions;
using UserCrud.Application.Interfaces;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.DeleteProduct;

public sealed class DeleteProductUseCase(
    IObjectStorageService objectStorageService, 
    IUnitOfWork unitOfWork) : IDeleteProductUseCase
{
    public async Task ExecuteAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Product.FindByIdAsync(productId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_NOT_FOUND);
        }
        
        var productImages = await unitOfWork.ProductImage.FindAllByProductIdAsync(productId, cancellationToken);

        foreach (var productImage in productImages)
        {
            await objectStorageService.DeleteAsync(productImage.ImageUrl, cancellationToken);
        }

        await unitOfWork.Product.DeleteAsync(product, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();
    }
}
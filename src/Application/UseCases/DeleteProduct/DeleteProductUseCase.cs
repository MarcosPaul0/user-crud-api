using UserCrud.Application.Exceptions;
using UserCrud.Application.Interfaces;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.DeleteProduct;

public class DeleteProductUseCase(
    IObjectStorageService objectStorageService,
    IProductRepository productRepository,
    IProductImageRepository productImageRepository,
    IUnitOfWork unitOfWork) : IDeleteProductUseCase
{
    public async Task ExecuteAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await productRepository.FindByIdAsync(productId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_NOT_FOUND);
        }
        
        var productImages = await productImageRepository.FindAllByProductIdAsync(productId, cancellationToken);

        foreach (var productImage in productImages)
        {
            await objectStorageService.DeleteAsync(productImage.ImageUrl, cancellationToken);
        }

        await productRepository.DeleteAsync(product, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();
    }
}
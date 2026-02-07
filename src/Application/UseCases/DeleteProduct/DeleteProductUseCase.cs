using UserCrud.Application.Exceptions;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.DeleteProduct;

public class DeleteProductUseCase(
    IProductRepository productRepository, 
    IUnitOfWork unitOfWork) : IDeleteProductUseCase
{
    public async Task ExecuteAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await productRepository.FindByIdAsync(productId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_NOT_FOUND);
        }
        
        await productRepository.DeleteAsync(product, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();
    }
}
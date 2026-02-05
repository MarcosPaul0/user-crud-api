using UserCrud.Application.Exceptions;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.FindProductById;

public class FindProductByIdUseCase(IProductRepository productRepository) : IFindProductByIdUseCase
{
    public async Task<Product> ExecuteAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await productRepository.FindByIdAsync(productId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_NOT_FOUND);
        }
        
        return product;
    }
}
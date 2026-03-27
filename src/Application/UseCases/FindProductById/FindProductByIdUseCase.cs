using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using UserCrud.Application.Exceptions;

namespace UserCrud.Application.UseCases.FindProductById;

public sealed class FindProductByIdUseCase(IUnitOfWork unitOfWork) : IFindProductByIdUseCase
{
    public async Task<Product> ExecuteAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Product.FindByIdAsync(productId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_NOT_FOUND);
        }
        
        return product;
    }
}
using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.UpdateProduct;

public interface IUpdateProductUseCase
{
    Task ExecuteAsync(Guid productId, UpdateProductDto updateProductDto, CancellationToken cancellationToken);
}
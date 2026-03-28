using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.UpdateProduct;

public interface IUpdateProductUseCase
{
    Task ExecuteAsync(Guid productId, UpdateProductDto updateProductDto, CancellationToken cancellationToken);
}
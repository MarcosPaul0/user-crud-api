using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.FindProductById;

public interface IFindProductByIdUseCase
{
    Task<Product> ExecuteAsync(Guid productId, CancellationToken cancellationToken);
}
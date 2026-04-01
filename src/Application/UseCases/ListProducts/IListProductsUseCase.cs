using AutoriaStore.Application.Dtos;
using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.ListProducts;

public interface IListProductsUseCase
{
    Task<(IEnumerable<Product> products, int count)> ExecuteAsync(ListProductsDto listProductsDto,
        CancellationToken cancellationToken);
}
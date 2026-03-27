using AutoriaStore.Domain.Entities;
using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.ListProducts;

public interface IListProductsUseCase
{
    Task<(IEnumerable<Product> products, int count)> ExecuteAsync(ListProductsDto listProductsDto,
        CancellationToken cancellationToken);
}
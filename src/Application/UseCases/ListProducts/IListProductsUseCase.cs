using UserCrud.Application.Dtos;
using UserCrud.Domain.Entities;

namespace UserCrud.Application.UseCases.ListProducts;

public interface IListProductsUseCase
{
    Task<(IEnumerable<Product> products, int count)> ExecuteAsync(ListProductsDto listProductsDto,
        CancellationToken cancellationToken);
}
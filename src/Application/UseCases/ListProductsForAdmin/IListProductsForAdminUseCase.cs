using AutoriaStore.Domain.Entities;
using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.ListProductsForAdmin;

public interface IListProductsForAdminUseCase
{
    Task<(IEnumerable<Product> products, int count)> ExecuteAsync(ListProductsByAdminDto listProductsByAdminDto,
        CancellationToken cancellationToken);
}
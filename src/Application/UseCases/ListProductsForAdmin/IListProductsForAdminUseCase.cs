using UserCrud.Application.Dtos;
using UserCrud.Domain.Entities;

namespace UserCrud.Application.UseCases.ListProductsForAdmin;

public interface IListProductsForAdminUseCase
{
    Task<(IEnumerable<Product> products, int count)> ExecuteAsync(ListProductsByAdminDto listProductsByAdminDto,
        CancellationToken cancellationToken);
}
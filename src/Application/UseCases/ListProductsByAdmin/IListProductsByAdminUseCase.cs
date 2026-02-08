using UserCrud.Application.Dtos;
using UserCrud.Domain.Entities;

namespace UserCrud.Application.UseCases.ListProductsByAdmin;

public interface IListProductsByAdminUseCase
{
    Task<(IEnumerable<Product> products, int count)> ExecuteAsync(ListProductsByAdminDto listProductsByAdminDto,
        CancellationToken cancellationToken);
}
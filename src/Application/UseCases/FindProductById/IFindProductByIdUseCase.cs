using UserCrud.Domain.Entities;

namespace UserCrud.Application.UseCases.FindProductById;

public interface IFindProductByIdUseCase
{
    Task<Product> ExecuteAsync(Guid productId, CancellationToken cancellationToken);
}
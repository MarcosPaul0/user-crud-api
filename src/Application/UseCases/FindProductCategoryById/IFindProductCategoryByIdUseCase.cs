using UserCrud.Domain.Entities;

namespace UserCrud.Application.UseCases.FindProductCategoryById;

public interface IFindProductCategoryByIdUseCase
{
    Task<ProductCategory> ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken);
}
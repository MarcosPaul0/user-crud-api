using UserCrud.Domain.Entities;

namespace UserCrud.Application.UseCases.ListProductCategory;

public interface IListProductCategoryUseCase
{
    Task<(IEnumerable<ProductCategory> productCategories, int count)> ExecuteAsync(CancellationToken cancellationToken);
}
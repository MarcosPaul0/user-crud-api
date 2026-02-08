using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.ListProductCategory;

public class ListProductCategoryUseCase(IProductCategoryRepository productCategoryRepository) : IListProductCategoryUseCase
{
    public async Task<(IEnumerable<ProductCategory> productCategories, int count)> ExecuteAsync(CancellationToken cancellationToken)
    {
        var productCategories = await productCategoryRepository.FindAllAsync(cancellationToken);

        return (productCategories, productCategories.Count());
    }
}
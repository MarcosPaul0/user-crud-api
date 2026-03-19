using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.ListProductCategory;

public sealed class ListProductCategoryUseCase(
    IUnitOfWork unitOfWork) : IListProductCategoryUseCase
{
    public async Task<(IEnumerable<ProductCategory> productCategories, int count)> ExecuteAsync(CancellationToken cancellationToken)
    {
        var productCategories = await unitOfWork.ProductCategory.FindAllAsync(cancellationToken);

        return (productCategories, productCategories.Count());
    }
}
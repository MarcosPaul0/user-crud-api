using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;

namespace AutoriaStore.Application.UseCases.ListProductCategory;

public sealed class ListProductCategoryUseCase(
    IUnitOfWork unitOfWork) : IListProductCategoryUseCase
{
    public async Task<(IEnumerable<ProductCategory> productCategories, int count)> ExecuteAsync(CancellationToken cancellationToken)
    {
        var productCategories = await unitOfWork.ProductCategory.FindAllAsync(cancellationToken);

        return (productCategories, productCategories.Count());
    }
}
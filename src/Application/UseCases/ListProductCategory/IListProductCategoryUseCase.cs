using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.ListProductCategory;

public interface IListProductCategoryUseCase
{
    Task<(List<ProductCategory> productCategories, int count)> ExecuteAsync(CancellationToken cancellationToken);
}
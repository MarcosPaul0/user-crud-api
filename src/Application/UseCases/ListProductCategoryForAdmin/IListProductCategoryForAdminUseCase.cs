using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.ListProductCategoryForAdmin;

public interface IListProductCategoryForAdminUseCase
{
    Task<(List<ProductCategory> productCategories, int count)> ExecuteAsync(CancellationToken cancellationToken);
}
using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces;

public interface IProductCategoryRepository : IBaseRepository<ProductCategory>
{
    Task<ProductCategory?> FindByCategoryAsync(string category, CancellationToken cancellationToken = default);

    Task<List<ProductCategory>> FindAllAsync(
        ProductCategory? filter,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
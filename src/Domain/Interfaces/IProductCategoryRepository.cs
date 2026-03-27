using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces;

public interface IProductCategoryRepository : IBaseRepository<ProductCategory>
{
    Task<ProductCategory?> FindByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<List<ProductCategory>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
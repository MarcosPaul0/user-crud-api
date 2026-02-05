using UserCrud.Domain.Entities;

namespace UserCrud.Domain.Interfaces;

public interface IProductCategoryRepository : IBaseRepository<ProductCategory>
{
    Task<ProductCategory?> FindByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductCategory>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
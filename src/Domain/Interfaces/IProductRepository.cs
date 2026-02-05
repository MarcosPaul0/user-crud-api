using UserCrud.Domain.Entities;

namespace UserCrud.Domain.Interfaces;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<IEnumerable<Product>> FindAllAsync(
        Product filter,
        int page,
        int itemsPerPage,
        CancellationToken cancellationToken = default);
    Task<int> CountAsync(Product filter, CancellationToken cancellationToken = default);
}
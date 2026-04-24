using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces.Repositories;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<Product?> FindByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<List<Product>> FindAllAsync(
        Product filter,
        int page,
        int itemsPerPage,
        CancellationToken cancellationToken = default);
    
    Task<int> CountAsync(Product filter, CancellationToken cancellationToken = default);
}
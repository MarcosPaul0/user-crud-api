using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces.Repositories;

public interface IProductImageRepository : IBaseRepository<ProductImage>
{
    Task<List<ProductImage>> FindAllByProductIdAsync(Guid productId,
        CancellationToken cancellationToken = default);
}
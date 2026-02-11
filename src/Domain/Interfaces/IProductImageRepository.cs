using UserCrud.Domain.Entities;

namespace UserCrud.Domain.Interfaces;

public interface IProductImageRepository : IBaseRepository<ProductImage>
{
    Task<List<ProductImage>> FindAllByProductIdAsync(Guid productId,
        CancellationToken cancellationToken = default);
}
namespace AutoriaStore.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository User { get; }
    IProductRepository Product { get; }
    IProductCategoryRepository ProductCategory { get; }
    IProductImageRepository ProductImage { get; }
    IOrderRepository Order { get; }
    IIdempotencyKeyRepository IdempotencyKey { get; }
    Task SaveChangesAsync();
}

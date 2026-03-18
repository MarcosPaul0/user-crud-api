namespace UserCrud.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository User { get; }
    IProductRepository Product { get; }
    IProductCategoryRepository ProductCategory { get; }
    IProductImageRepository ProductImage { get; }
    Task SaveChangesAsync();
}
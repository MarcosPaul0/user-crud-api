using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace AutoriaStore.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private readonly Lazy<IUserRepository> _user;
    private readonly Lazy<IProductRepository> _product;
    private readonly Lazy<IProductCategoryRepository> _productCategory;
    private readonly Lazy<IProductImageRepository> _productImage;
    private readonly Lazy<IOrderRepository> _order;
    private readonly Lazy<IIdempotencyKeyRepository> _idempotencyKey;

    public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        
        _user = new Lazy<IUserRepository>(
            serviceProvider.GetRequiredService<IUserRepository>);
        _product = new Lazy<IProductRepository>(
            serviceProvider.GetRequiredService<IProductRepository>);
        _productCategory = new Lazy<IProductCategoryRepository>(
            serviceProvider.GetRequiredService<IProductCategoryRepository>);
        _productImage = new Lazy<IProductImageRepository>(
            serviceProvider.GetRequiredService<IProductImageRepository>);
        _order = new Lazy<IOrderRepository>(
            serviceProvider.GetRequiredService<IOrderRepository>);
        _idempotencyKey = new Lazy<IIdempotencyKeyRepository>(
            serviceProvider.GetRequiredService<IIdempotencyKeyRepository>);
    }
    
    public IUserRepository User => _user.Value;
    public IProductRepository Product => _product.Value;
    public IProductCategoryRepository ProductCategory => _productCategory.Value;
    public IProductImageRepository ProductImage => _productImage.Value;
    public IOrderRepository Order => _order.Value;
    public IIdempotencyKeyRepository IdempotencyKey => _idempotencyKey.Value;
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}

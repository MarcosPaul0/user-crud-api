using AutoriaStore.Domain.Interfaces;
using AutoriaStore.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;

namespace UserCrud.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private readonly Lazy<IUserRepository> _user;
    private readonly Lazy<IProductRepository> _product;
    private readonly Lazy<IProductCategoryRepository> _productCategory;
    private readonly Lazy<IProductImageRepository> _productImage;

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
    }
    
    public IUserRepository User => _user.Value;
    public IProductRepository Product => _product.Value;
    public IProductCategoryRepository ProductCategory => _productCategory.Value;
    public IProductImageRepository ProductImage => _productImage.Value;
    
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
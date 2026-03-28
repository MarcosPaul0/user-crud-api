using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using AutoriaStore.Infrastructure.Context;
using AutoriaStore.Infrastructure.Repositories.FilterBuilders;
using Microsoft.EntityFrameworkCore;

namespace AutoriaStore.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
{
    private readonly ApplicationDbContext _context = context;

    public override async Task<Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Product
            .Include(product => product.ProductCategory)
            .Include(product => product.ProductImages.OrderBy(productImage => productImage.DisplayOrder))
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    public async Task<Product?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Product.FirstOrDefaultAsync(product => product.Name == name, cancellationToken);
    }
    
    public async Task<IEnumerable<Product>> FindAllAsync(
        Product filter, 
        int page, 
        int itemsPerPage, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Product.AsNoTracking();

        query = new ProductFilterBuilder(query)
            .FilterByIsActive(filter.IsActive)
            .FilterByName(filter.Name)
            .FilterByProductCategoryId(filter.ProductCategoryId)
            .ApplyPagination(page, itemsPerPage)
            .Build();
        
        return await query
            .Include(product => product.ProductCategory)
            .Include(product => product.ProductImages.OrderBy(productImage => productImage.DisplayOrder))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Product filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Product.AsNoTracking();

        query = new ProductFilterBuilder(query)
            .FilterByIsActive(filter.IsActive)
            .FilterByName(filter.Name)
            .FilterByProductCategoryId(filter.ProductCategoryId)
            .Build();
        
        return await query.CountAsync(cancellationToken);
    }
}
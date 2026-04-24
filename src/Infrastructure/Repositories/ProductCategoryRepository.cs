using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;
using AutoriaStore.Infrastructure.Repositories.FilterBuilders;
using Microsoft.EntityFrameworkCore;

namespace AutoriaStore.Infrastructure.Repositories;

public class ProductCategoryRepository(ApplicationDbContext context) : BaseRepository<ProductCategory>(context), IProductCategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<ProductCategory?> FindByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return _context.ProductCategory.FirstOrDefaultAsync(productCategory => productCategory.Category == category,
            cancellationToken);
    }

    public async Task<List<ProductCategory>> FindAllAsync(
        ProductCategory? filter, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.ProductCategory.AsNoTracking();

        if (filter is not null)
        {
            query = new ProductCategoryFilterBuilder(query)
                .FilterByCategory(filter.Category)
                .FilterByIsActive(filter.IsActive)
                .Build();
        }
        
        return await query.ToListAsync(cancellationToken);
    }
    
    public async Task<List<ProductCategory>> FindAllWithProductCountAsync(
        ProductCategory? filter, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.ProductCategory.AsNoTracking();

        if (filter is not null)
        {
            query = new ProductCategoryFilterBuilder(query)
                .FilterByCategory(filter.Category)
                .FilterByIsActive(filter.IsActive)
                .Build();
        }
        
        return await query
            .Select(productCategory => new ProductCategory
            {
                Id = productCategory.Id,
                Category = productCategory.Category,
                ProductCount = _context.Product.Count(p => p.ProductCategoryId == productCategory.Id),
                CreatedAt = productCategory.CreatedAt,
                UpdatedAt = productCategory.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategory.AsNoTracking().CountAsync(cancellationToken);
    }
}
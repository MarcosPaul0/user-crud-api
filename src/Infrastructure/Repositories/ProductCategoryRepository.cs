using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using AutoriaStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using UserCrud.Infrastructure.Repositories;

namespace AutoriaStore.Infrastructure.Repositories;

public class ProductCategoryRepository(ApplicationDbContext context) : BaseRepository<ProductCategory>(context), IProductCategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<ProductCategory?> FindByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return _context.ProductCategory.FirstOrDefaultAsync(productCategory => productCategory.Category == category,
            cancellationToken);
    }

    public async Task<List<ProductCategory>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategory
            .AsNoTracking()
            .Where(productCategory => productCategory.IsActive)
            .Select(productCategory => new ProductCategory(
                productCategory.Id,
                productCategory.Category,
                _context.Product.Count(p => p.ProductCategoryId == productCategory.Id),
                productCategory.CreatedAt,
                productCategory.UpdatedAt
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategory.AsNoTracking().CountAsync(cancellationToken);
    }
}
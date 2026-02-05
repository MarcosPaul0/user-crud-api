using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;

namespace UserCrud.Infrastructure.Repositories;

public class ProductCategoryRepository(ApplicationDbContext context) : BaseRepository<ProductCategory>(context), IProductCategoryRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task<ProductCategory?> FindByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        return _context.ProductCategory.FirstOrDefaultAsync(productCategory => productCategory.Category == category,
            cancellationToken);
    }

    public async Task<IEnumerable<ProductCategory>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategory.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategory.AsNoTracking().CountAsync(cancellationToken);
    }
}
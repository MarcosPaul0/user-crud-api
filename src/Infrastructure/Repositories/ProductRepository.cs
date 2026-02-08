using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;
using UserCrud.Infrastructure.Repositories.FilterBuilders;

namespace UserCrud.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : BaseRepository<Product>(context), IProductRepository
{
    private readonly ApplicationDbContext _context = context;
    
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
        
        return await query.ToListAsync(cancellationToken);
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
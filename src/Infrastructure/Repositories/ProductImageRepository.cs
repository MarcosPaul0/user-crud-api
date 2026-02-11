using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;

namespace UserCrud.Infrastructure.Repositories;

public class ProductImageRepository(ApplicationDbContext context) : BaseRepository<ProductImage>(context), IProductImageRepository
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<List<ProductImage>> FindAllByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductImage.AsNoTracking().Where(productImage => productImage.ProductId == productId)
            .ToListAsync(cancellationToken);
    }
}
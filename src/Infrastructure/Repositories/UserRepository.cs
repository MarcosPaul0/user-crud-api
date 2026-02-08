using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;
using UserCrud.Infrastructure.Repositories.FilterBuilders;

namespace UserCrud.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<User>> FindAllAsync(
        User filter, 
        int page, 
        int itemsPerPage, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.User.AsNoTracking();

        query = new UserFilterBuilder(query)
            .FilterByName(filter.Name)
            .FilterByRole(filter.Role)
            .ApplyPagination(page, itemsPerPage)
            .Build();
        
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(User filter, CancellationToken cancellationToken = default)
    {
        var query = _context.User.AsNoTracking();

        query = new UserFilterBuilder(query)
            .FilterByName(filter.Name)
            .FilterByRole(filter.Role)
            .Build();
        
        return await query.CountAsync(cancellationToken);
    }


    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.User.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<User?> FindWithPonesByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.User.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }
}
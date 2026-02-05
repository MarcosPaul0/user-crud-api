using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;

namespace UserCrud.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<User>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.User.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.User.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<User?> FindWithPonesByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.User
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }
}
using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;

namespace UserCrud.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(user, cancellationToken);
        
        return user;
    }

    public async Task<User> DeleteAsync(User user)
    {
        context.User.Remove(user);
        
        return await Task.FromResult(user);
    }

    public async Task<IEnumerable<User>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.User.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.User.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.User.FindAsync(id, cancellationToken);
    }

    public async Task<User?> FindWithPonesByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.User
            .Include(user => user.Phones)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public async Task<User> UpdateAsync(User user)
    {
        context.Update(user);
        
        return await Task.FromResult(user);
    }
}
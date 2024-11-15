using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;

namespace UserCrud.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _userContext;

        public UserRepository(ApplicationDbContext context)
        {
            _userContext = context;
        }

        public async Task<User> Create(User user)
        {
            _userContext.Add(user);
            await _userContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> Delete(User user)
        {
            _userContext.Remove(user);
            await _userContext.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> FindAll()
        {
            return await _userContext.User.AsNoTracking().ToListAsync();
        }

        public async Task<User?> FindById(string id)
        {
            return await _userContext.User.FindAsync(id);
        }

        public async Task<User?> FindWithPonesById(string id)
        {
            return await _userContext.User
                .Include(user => user.Phones)
                .FirstOrDefaultAsync(user => user.Id == Guid.Parse(id));
        }

        public async Task<User> Update(User user)
        {
            _userContext.Update(user);
            await _userContext.SaveChangesAsync();
            return user;
        }
    }
}
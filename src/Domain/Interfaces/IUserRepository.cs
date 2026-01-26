using UserCrud.Domain.Entities;

namespace UserCrud.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> FindAllAsync(CancellationToken cancellationToken= default);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken= default);
    Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken= default);
    Task<User?> FindWithPonesByIdAsync(Guid id, CancellationToken cancellationToken= default);
    Task<User> CreateAsync(User user, CancellationToken cancellationToken= default);
    Task<User> UpdateAsync(User user);
    Task<User> DeleteAsync(User user);
}
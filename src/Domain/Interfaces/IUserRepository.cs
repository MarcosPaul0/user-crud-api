using UserCrud.Domain.Entities;

namespace UserCrud.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<IEnumerable<User>> FindAllAsync(CancellationToken cancellationToken= default);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken= default);
}
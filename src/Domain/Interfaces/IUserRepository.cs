using UserCrud.Domain.Entities;

namespace UserCrud.Domain.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<IEnumerable<User>> FindAllAsync(
        User filter, 
        int page, 
        int itemsPerPage, 
        CancellationToken cancellationToken= default);
    Task<int> CountAsync(User filter, CancellationToken cancellationToken= default);
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken= default);
}
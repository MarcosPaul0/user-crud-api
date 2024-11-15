using UserCrud.Domain.Entities;

namespace UserCrud.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> FindAll();
        Task<User?> FindById(string id);
        Task<User?> FindWithPonesById(string id);
        Task<User> Create(User user);
        Task<User> Update(User user);
        Task<User> Delete(User user);
    }
}
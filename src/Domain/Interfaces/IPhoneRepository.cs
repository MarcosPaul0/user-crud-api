

using UserCrud.Domain.Entities;

namespace UserCrud.Domain.Interfaces
{
    public interface IPhoneRepository
    {
        Task<IEnumerable<Phone>> FindAll();
        Task<Phone?> FindById(string id);
        Task<IEnumerable<Phone>> FindByUserId(string userId);
        Task<Phone> Create(Phone phone);
        Task<Phone> Update(Phone phone);
        Task<Phone> Delete(Phone phone);
    }
}
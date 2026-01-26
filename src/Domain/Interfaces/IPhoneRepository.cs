using UserCrud.Domain.Entities;

namespace UserCrud.Domain.Interfaces;

public interface IPhoneRepository
{
    Task<IEnumerable<Phone>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<Phone?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Phone>> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Phone> CreateAsync(Phone phone, CancellationToken cancellationToken = default);
    Task<Phone> UpdateAsync(Phone phone);
    Task<Phone> DeleteAsync(Phone phone);
}
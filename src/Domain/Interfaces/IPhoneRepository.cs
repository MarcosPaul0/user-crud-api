using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces;

public interface IPhoneRepository : IBaseRepository<Phone>
{
    Task<IEnumerable<Phone>> FindAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Phone>> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}
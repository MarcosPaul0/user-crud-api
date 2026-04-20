using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces;

public interface IIdempotencyKeyRepository : IBaseRepository<IdempotencyKey>
{
    Task<IdempotencyKey?> FindByUserIdAndEndpointAndKeyAsync(
        Guid userId,
        string endpoint,
        string idempotencyKey,
        CancellationToken cancellationToken = default);
}

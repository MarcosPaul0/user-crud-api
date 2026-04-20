using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using AutoriaStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AutoriaStore.Infrastructure.Repositories;

public sealed class IdempotencyKeyRepository : BaseRepository<IdempotencyKey>, IIdempotencyKeyRepository
{
    private readonly ApplicationDbContext _context;

    public IdempotencyKeyRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IdempotencyKey?> FindByUserIdAndEndpointAndKeyAsync(
        Guid userId,
        string endpoint,
        string idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        return await _context.IdempotencyKey.FirstOrDefaultAsync(
            record => record.UserId == userId
                      && record.Endpoint == endpoint
                      && record.Key == idempotencyKey,
            cancellationToken);
    }
}

// <copyright file="IdempotencyKeyRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AutoriaStore.Infrastructure.Repositories;

public sealed class IdempotencyKeyRepository : BaseRepository<IdempotencyKey>, IIdempotencyKeyRepository
{
    private readonly ApplicationDbContext _context;

    public IdempotencyKeyRepository(ApplicationDbContext context)
        : base(context)
    {
        this._context = context;
    }

    public async Task<IdempotencyKey?> FindByUserIdAndEndpointAndKeyAsync(
        Guid userId,
        string endpoint,
        string idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        return await this._context.IdempotencyKey.FirstOrDefaultAsync(
            record => record.UserId == userId
                      && record.Endpoint == endpoint
                      && record.Key == idempotencyKey,
            cancellationToken);
    }
}

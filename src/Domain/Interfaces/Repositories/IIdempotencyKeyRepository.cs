// <copyright file="IIdempotencyKeyRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces.Repositories;

public interface IIdempotencyKeyRepository : IBaseRepository<IdempotencyKey>
{
    Task<IdempotencyKey?> FindByUserIdAndEndpointAndKeyAsync(
        Guid userId,
        string endpoint,
        string idempotencyKey,
        CancellationToken cancellationToken = default);
}

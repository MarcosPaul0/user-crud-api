// <copyright file="IIdempotencyService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces.Services;

public interface IIdempotencyService
{
    Task<IdempotencyKey?> GetIdempotencyKeyAsync(
        Guid authenticatedUserId,
        string idempotencyKey,
        string endpoint,
        CancellationToken cancellationToken = default);

    Task CreateIdempotencyKeyAsync(
        CreateIdempotencyKeyDto createIdempotencyKeyDto,
        CancellationToken cancellationToken = default);

    Task<bool> RemoveIdempotencyKeyIfExpiredAsync(
        IdempotencyKey idempotencyKey,
        CancellationToken cancellationToken = default);
}
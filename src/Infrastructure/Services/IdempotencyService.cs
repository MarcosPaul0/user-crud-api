// <copyright file="IdempotencyService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.Infrastructure.Services;

public sealed class IdempotencyService(IUnitOfWork unitOfWork) : IIdempotencyService
{
    public async Task<IdempotencyKey?> GetIdempotencyKeyAsync(
        Guid authenticatedUserId,
        string idempotencyKey,
        string endpoint,
        CancellationToken cancellationToken = default)
    {
        var normalizedIdempotencyKey = idempotencyKey.Trim();
        var normalizedEndpoint = endpoint.Trim();

        var existingIdempotencyKey = await unitOfWork.IdempotencyKey.FindByUserIdAndEndpointAndKeyAsync(
            authenticatedUserId,
            normalizedEndpoint,
            normalizedIdempotencyKey,
            cancellationToken);

        return existingIdempotencyKey;
    }

    public async Task CreateIdempotencyKeyAsync(
        CreateIdempotencyKeyDto createIdempotencyKeyDto,
        CancellationToken cancellationToken = default)
    {
        var requestHash = GenerateHash(createIdempotencyKeyDto.RequestObject);
        var responseJson = SerializeData(createIdempotencyKeyDto.ResponseObject);

        var normalizedIdempotencyKey = createIdempotencyKeyDto.IdempotencyKey.Trim();
        var normalizedEndpoint = createIdempotencyKeyDto.Endpoint.Trim();

        var idempotencyKeyEntry = new IdempotencyKey
        {
            Id = Guid.NewGuid(),
            UserId = createIdempotencyKeyDto.AuthenticatedUserId,
            Key = normalizedIdempotencyKey,
            Endpoint = normalizedEndpoint,
            RequestHash = requestHash,
            ResponseStatus = createIdempotencyKeyDto.StatusCode,
            ResponseBody = responseJson,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            CreatedAt = DateTime.UtcNow,
        };

        await unitOfWork.IdempotencyKey.CreateAsync(idempotencyKeyEntry, cancellationToken);
    }

    private static string GenerateHash(object data)
    {
        var payloadJson = JsonSerializer.Serialize(data);
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(payloadJson));

        return Convert.ToHexString(hashBytes);
    }

    private static string? SerializeData(object? data)
    {
        if (data is null)
        {
            return null;
        }

        return JsonSerializer.Serialize(data);
    }

    public async Task<bool> RemoveIdempotencyKeyIfExpiredAsync(
        IdempotencyKey idempotencyKey,
        CancellationToken cancellationToken = default)
    {
        if (idempotencyKey.ExpiresAt > DateTime.UtcNow)
        {
            return false;
        }

        await unitOfWork.IdempotencyKey.DeleteAsync(idempotencyKey, cancellationToken);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}
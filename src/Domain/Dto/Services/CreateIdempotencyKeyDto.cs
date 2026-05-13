// <copyright file="CreateIdempotencyKeyDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Services;

public record CreateIdempotencyKeyDto
{
    required public Guid AuthenticatedUserId { get; init; }
    required public string IdempotencyKey { get; init; }
    required public string Endpoint { get; init; }
    required public int StatusCode { get; init; }
    required public object RequestObject { get; init; }
    required public object? ResponseObject { get; init; }
}
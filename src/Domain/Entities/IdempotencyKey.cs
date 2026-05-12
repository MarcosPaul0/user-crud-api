// <copyright file="IdempotencyKey.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Entities;

public class IdempotencyKey : Entity
{
    public Guid UserId { get; set; }

    public string Key { get; set; } = null!;

    public string Endpoint { get; set; } = null!;

    public string RequestHash { get; set; } = null!;

    public int ResponseStatus { get; set; }

    public string? ResponseBody { get; set; }

    public DateTime ExpiresAt { get; set; }
}

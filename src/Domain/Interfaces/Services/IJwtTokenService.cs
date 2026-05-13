// <copyright file="IJwtTokenService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Enums;

namespace AutoriaStore.Domain.Interfaces.Services;

public interface IJwtTokenService
{
    public string GenerateToken(Guid userId, UserRole role);
}
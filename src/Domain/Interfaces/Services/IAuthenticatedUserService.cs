// <copyright file="IAuthenticatedUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Interfaces.Services;

public interface IAuthenticatedUserService
{
    Guid? GetUserId();
}

// <copyright file="IPasswordHasherService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Interfaces.Services;

public interface IPasswordHasherService
{
    string Hash(string password);

    bool Verify(string password, string hash);
}
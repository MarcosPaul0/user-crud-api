// <copyright file="ILoginUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.Login;

public interface ILoginUseCase
{
    Task<string> ExecuteAsync(LoginDto loginDto, CancellationToken cancellationToken);
}
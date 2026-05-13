// <copyright file="IUpdateUserUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.UpdateUser;

public interface IUpdateUserUseCase
{
    Task ExecuteAsync(Guid userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken);
}
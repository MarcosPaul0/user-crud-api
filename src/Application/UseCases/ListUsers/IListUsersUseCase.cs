// <copyright file="IListUsersUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.ListUsers;

public interface IListUserUseCase
{
    Task<(IEnumerable<User>, int)> ExecuteAsync(ListUsersDto listUsersDto, CancellationToken cancellationToken);
}
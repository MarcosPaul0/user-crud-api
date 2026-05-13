// <copyright file="IFindUserByIdUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.FindUserById;

public interface IFindUserByIdUseCase
{
    Task<User> ExecuteAsync(Guid userId, CancellationToken cancellationToken);
}
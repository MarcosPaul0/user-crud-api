// <copyright file="IDeleteUserUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.UseCases.DeleteUser;

public interface IDeleteUserUseCase
{
    Task ExecuteAsync(Guid userId, CancellationToken cancellationToken);
}
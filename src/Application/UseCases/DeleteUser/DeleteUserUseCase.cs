// <copyright file="DeleteUserUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.DeleteUser;

public sealed class DeleteUserUseCase(IUnitOfWork unitOfWork) : IDeleteUserUseCase
{
    public async Task ExecuteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.User.FindByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(ExceptionMessages.USER_NOT_FOUND);
        }

        await unitOfWork.User.DeleteAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync();
    }
}
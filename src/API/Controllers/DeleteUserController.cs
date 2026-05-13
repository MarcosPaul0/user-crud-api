// <copyright file="DeleteUserController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.UseCases.DeleteUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/user")]
public class DeleteUserController(IDeleteUserUseCase deleteUserUseCase) : ControllerBase
{
    [Authorize]
    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult> HandleAsync(Guid userId, CancellationToken cancellationToken)
    {
        await deleteUserUseCase.ExecuteAsync(userId, cancellationToken);

        return this.NoContent();
    }
}
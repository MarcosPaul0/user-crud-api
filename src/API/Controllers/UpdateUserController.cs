// <copyright file="UpdateUserController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.UpdateUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/user")]
public class UpdateUserController(IUpdateUserUseCase updateUserUseCase) : ControllerBase
{
    [Authorize]
    [HttpPatch("{userId:guid}")]
    public async Task<IActionResult> Handle(
        Guid userId,
        [FromBody] UpdateUserDto updateUserDto,
        CancellationToken cancellationToken)
    {
        await updateUserUseCase.ExecuteAsync(userId, updateUserDto, cancellationToken);

        return this.NoContent();
    }
}
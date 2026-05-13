// <copyright file="FindUserByIdController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using AutoriaStore.Application.UseCases.FindUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/user")]
public class FindUserByIdController(IFindUserByIdUseCase findUserByIdUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> Handle([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var user = await findUserByIdUseCase.ExecuteAsync(userId, cancellationToken);

        return this.Ok(UserPresenter.ToHttp(user));
    }
}
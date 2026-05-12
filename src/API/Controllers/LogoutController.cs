// <copyright file="LogoutController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/auth/logout")]
public class LogoutController(IEnvironmentVariablesService environmentVariablesService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
        var authTokenCookie = environmentVariablesService.AuthTokenCookie;

        this.Response.Cookies.Delete(authTokenCookie);

        return this.NoContent();
    }
}
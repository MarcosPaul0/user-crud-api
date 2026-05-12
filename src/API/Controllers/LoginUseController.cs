// <copyright file="LoginUseController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.Login;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/auth/login")]
public class LoginUseController(ILoginUseCase loginUseCase, IEnvironmentVariablesService environmentVariablesService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] LoginDto loginDto,
        CancellationToken cancellationToken)
    {
        var token = await loginUseCase.ExecuteAsync(loginDto, cancellationToken);

        var authTokenCookie = environmentVariablesService.AuthTokenCookie;

        this.Response.Cookies.Append(authTokenCookie, token, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddHours(4),
        });

        return this.NoContent();
    }
}
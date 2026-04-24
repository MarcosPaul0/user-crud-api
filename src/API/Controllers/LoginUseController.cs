using AutoriaStore.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.UseCases.Login;
using AutoriaStore.Domain.Interfaces.Services;

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
        
        Response.Cookies.Append(authTokenCookie, token, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddHours(4)
        });
        
        return NoContent();
    }
}
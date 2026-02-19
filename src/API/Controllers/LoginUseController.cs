using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Dtos;
using UserCrud.Application.Interfaces;
using UserCrud.Application.UseCases.Login;

namespace UserCrud.API.Controllers;

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
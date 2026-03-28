using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.Interfaces;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/auth/logout")]
public class LogoutController(IEnvironmentVariablesService environmentVariablesService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
        var authTokenCookie = environmentVariablesService.AuthTokenCookie;
        
        Response.Cookies.Delete(authTokenCookie);
        
        return NoContent();
    }
}
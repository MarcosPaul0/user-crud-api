using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Interfaces;

namespace UserCrud.API.Controllers;

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
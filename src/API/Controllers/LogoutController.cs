using Microsoft.AspNetCore.Mvc;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/auth/logout")]
public class LogoutController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
        Response.Cookies.Delete("autoria_token");
        
        return Ok();
    }
}
using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.Login;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/auth/login")]
public class LoginUseController(ILoginUseCase loginUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] LoginDto loginDto,
        CancellationToken cancellationToken)
    {
        var token = await loginUseCase.ExecuteAsync(loginDto, cancellationToken);

        Response.Cookies.Append("autoria_token", token);
        
        return NoContent();
    }
}
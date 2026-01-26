using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.CreateUser;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/user")]
public class CreateUserController(ICreateUserUseCase createUserUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] CreateUserDto createUserDto,
        CancellationToken cancellationToken)
    {
        await createUserUseCase.ExecuteAsync(createUserDto, cancellationToken);

        return Created();
    }
}
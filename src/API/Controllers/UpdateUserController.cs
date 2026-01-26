using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.UpdateUser;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/user")]
public class UpdateUserController(IUpdateUserUseCase updateUserUseCase) : ControllerBase
{
    [Authorize]
    [HttpPatch("{userId:guid}")]
    public async Task<IActionResult> Handle(
        Guid userId, 
        [FromBody] UpdateUserDto updateUserDto, 
        CancellationToken cancellationToken)
    {
        await updateUserUseCase.ExecuteAsync(userId, updateUserDto, cancellationToken);

        return NoContent();
    }
}
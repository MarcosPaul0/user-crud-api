using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.UseCases.DeleteUser;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/user")]
public class DeleteUserController(IDeleteUserUseCase deleteUserUseCase) : ControllerBase
{
    [Authorize]
    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult> HandleAsync(Guid userId, CancellationToken cancellationToken)
    {
        await deleteUserUseCase.ExecuteAsync(userId, cancellationToken);
        
        return NoContent();
    }
}
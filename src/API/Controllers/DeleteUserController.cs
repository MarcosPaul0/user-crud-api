using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.UseCases.DeleteUser;

namespace UserCrud.API.Controllers;

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
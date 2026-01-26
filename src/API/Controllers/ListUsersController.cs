using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.UseCases.ListUsers;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/user")]
public class FindAllUsersController(IListUserUseCase listUsersUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> HandleAsync(CancellationToken cancellationToken)
    {
        var result = await listUsersUseCase.ExecuteAsync(cancellationToken);

        return Ok(result);
    }
}
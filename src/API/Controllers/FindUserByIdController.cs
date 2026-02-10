using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Presenters;
using UserCrud.Application.UseCases.FindUserById;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/user")]
public class FindUserByIdController(IFindUserByIdUseCase findUserByIdUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult> Handle([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var user = await findUserByIdUseCase.ExecuteAsync(userId, cancellationToken);

        return Ok(UserPresenter.ToHttp(user));
    }
}
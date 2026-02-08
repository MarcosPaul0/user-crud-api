using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Presenters;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.ListUsers;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/user")]
public class FindAllUsersController(IListUserUseCase listUsersUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> HandleAsync(
        ListUsersDto listUsersDto,
        CancellationToken cancellationToken)
    {
        var (users, usersCount) = await listUsersUseCase.ExecuteAsync(listUsersDto, cancellationToken);

        return Ok(UserPresenter.ToHttp(users, usersCount, listUsersDto.Page, listUsersDto.ItemsPerPage));
    }
}
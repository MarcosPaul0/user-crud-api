using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using AutoriaStore.Application.Dtos;
using AutoriaStore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.UseCases.ListUsers;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/user")]
public class FindAllUsersController(IListUserUseCase listUsersUseCase) : ControllerBase
{
    [Authorize]
    [HttpPost("list")]
    [ProducesResponseType(typeof(PaginationResponseDto<UserResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<User>>> HandleAsync(
        ListUsersDto listUsersDto,
        CancellationToken cancellationToken)
    {
        var (users, usersCount) = await listUsersUseCase.ExecuteAsync(listUsersDto, cancellationToken);

        return Ok(UserPresenter.ToHttp(users, usersCount, listUsersDto.Page, listUsersDto.ItemsPerPage));
    }
}
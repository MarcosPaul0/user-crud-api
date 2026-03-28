using AutoriaStore.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.UseCases.CreateUser;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/user")]
public class CreateCustomerController(ICreateCustomerUseCase createCustomerUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] CreateUserDto createUserDto,
        CancellationToken cancellationToken)
    {
        await createCustomerUseCase.ExecuteAsync(createUserDto, cancellationToken);

        return Created();
    }
}
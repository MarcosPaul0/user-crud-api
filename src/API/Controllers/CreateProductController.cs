using AutoriaStore.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.UseCases.CreateProduct;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class CreateProductController(ICreateProductUseCase createProductUseCase) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] CreateProductDto createProductDto,
        CancellationToken cancellationToken)
    {
        await createProductUseCase.ExecuteAsync(createProductDto, cancellationToken);

        return Created();
    }
}
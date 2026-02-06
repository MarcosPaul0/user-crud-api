using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.CreateProduct;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product")]
public class CreateProductController(ICreateProductUseCase createProductUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] CreateProductDto createProductDto,
        CancellationToken cancellationToken)
    {
        await createProductUseCase.ExecuteAsync(createProductDto, cancellationToken);

        return Created();
    }
}
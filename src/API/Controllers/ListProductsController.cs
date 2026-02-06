using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.ListProducts;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product")]
public class ListProductsController(IListProductsUseCase listProductsUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> HandleAsync(
        [FromBody] ListProductDto listProductDto,
        CancellationToken cancellationToken)
    {
        var result = await listProductsUseCase.ExecuteAsync(listProductDto, cancellationToken);

        return Ok();
    }
}
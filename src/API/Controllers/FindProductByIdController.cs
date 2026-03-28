using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.UseCases.FindProductById;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class FindProductByIdController(IFindProductByIdUseCase findProductByIdUseCase) : ControllerBase
{
    [HttpGet("{productId:guid}")]
    [ProducesResponseType(typeof(ProductByIdResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var product = await findProductByIdUseCase.ExecuteAsync(productId, cancellationToken);

        return Ok(ProductPresenter.ToHttp(product));
    }
}
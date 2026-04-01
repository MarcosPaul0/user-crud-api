using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.UseCases.FindProductById;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class FindProductByIdForAdminController(IFindProductByIdUseCase findProductByIdUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet("for-admin/{productId:guid}")]
    [ProducesResponseType(typeof(ProductForAdminResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var product = await findProductByIdUseCase.ExecuteAsync(productId, cancellationToken);

        return Ok(ProductForAdminPresenter.ToHttp(product));
    }
}
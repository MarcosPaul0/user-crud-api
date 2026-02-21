using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Presenters;
using UserCrud.Application.UseCases.FindProductById;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product")]
public class FindProductByIdForAdminController(IFindProductByIdUseCase findProductByIdUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet("for-admin/{productId:guid}")]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var product = await findProductByIdUseCase.ExecuteAsync(productId, cancellationToken);

        return Ok(ProductForAdminPresenter.ToHttp(product));
    }
}
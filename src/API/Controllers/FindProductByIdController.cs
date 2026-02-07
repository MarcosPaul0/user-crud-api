using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Presenters;
using UserCrud.Application.UseCases.FindProductById;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product")]
public class FindProductByIdController(IFindProductByIdUseCase findProductByIdUseCase) : ControllerBase
{
    [HttpGet("{productId:guid}")]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var product = await findProductByIdUseCase.ExecuteAsync(productId, cancellationToken);

        return Ok(ProductPresenter.ToHttp(product));
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Presenters;
using UserCrud.Application.UseCases.FindProductCategoryById;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class FindProductCategoryByIdController(IFindProductCategoryByIdUseCase findProductCategoryByIdUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet("{productCategoryId:guid}")]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productCategoryId,
        CancellationToken cancellationToken)
    {
        var productCategory = await findProductCategoryByIdUseCase.ExecuteAsync(productCategoryId, cancellationToken);

        return Ok(ProductCategoryPresenter.ToHttp(productCategory));
    }
}
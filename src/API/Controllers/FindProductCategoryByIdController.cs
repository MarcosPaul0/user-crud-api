using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.UseCases.FindProductCategoryById;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class FindProductCategoryByIdController(IFindProductCategoryByIdUseCase findProductCategoryByIdUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet("{productCategoryId:guid}")]
    [ProducesResponseType(typeof(ProductCategoryResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productCategoryId,
        CancellationToken cancellationToken)
    {
        var productCategory = await findProductCategoryByIdUseCase.ExecuteAsync(productCategoryId, cancellationToken);

        return Ok(ProductCategoryPresenter.ToHttp(productCategory));
    }
}
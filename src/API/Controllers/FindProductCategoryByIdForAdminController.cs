using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using AutoriaStore.Application.UseCases.FindProductCategoryByIdForAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class FindProductCategoryByIdForAdminController(
    IFindProductCategoryByIdForAdminUseCase findProductCategoryByIdForAdminUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet("for-admin/{productCategoryId:guid}")]
    [ProducesResponseType(typeof(ProductCategoryResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productCategoryId,
        CancellationToken cancellationToken)
    {
        var productCategory = await findProductCategoryByIdForAdminUseCase.ExecuteAsync(productCategoryId, cancellationToken);

        return Ok(ProductCategoryForAdminPresenter.ToHttp(productCategory));
    }
}
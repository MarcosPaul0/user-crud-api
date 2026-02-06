using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.UpdateProductCategory;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class UpdateProductCategoryController(IUpdateProductCategoryUseCase updateProductCategoryUseCase) : ControllerBase
{
    [HttpPatch("{productCategoryId:guid}")]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productCategoryId,
        [FromBody] UpdateProductCategoryDto updateProductCategoryDto,
        CancellationToken cancellationToken)
    {
        await updateProductCategoryUseCase.ExecuteAsync(productCategoryId, updateProductCategoryDto, cancellationToken);

        return Ok();
    }
}
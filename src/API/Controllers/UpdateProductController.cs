using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.UpdateProduct;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product")]
public class UpdateProductController(IUpdateProductUseCase updateProductUseCase) : ControllerBase
{
    [HttpPatch("{productId:guid}")]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        [FromBody] UpdateProductDto updateProductDto,
        CancellationToken cancellationToken)
    {
        await updateProductUseCase.ExecuteAsync(productId, updateProductDto, cancellationToken);

        return Ok();
    }
}
using AutoriaStore.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.UseCases.UpdateProduct;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class UpdateProductController(IUpdateProductUseCase updateProductUseCase) : ControllerBase
{
    [Authorize]
    [HttpPatch("{productId:guid}")]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        [FromBody] UpdateProductDto updateProductDto,
        CancellationToken cancellationToken)
    {
        await updateProductUseCase.ExecuteAsync(productId, updateProductDto, cancellationToken);

        return NoContent();
    }
}
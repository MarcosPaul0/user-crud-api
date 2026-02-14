using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.SetProductImages;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product")]
public class SetProductImagesController(ISetProductImagesUseCase setProductImagesUseCase) : ControllerBase
{
    private const int ProductImageMaxSize = 15_728_640;
    
    [Authorize]
    [HttpPatch("images/{productId:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ProductImageMaxSize)]
    [RequestFormLimits(MultipartBodyLengthLimit = ProductImageMaxSize)]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        [FromForm] SetProductImagesDto setProductImagesDto,
        CancellationToken cancellationToken)
    {
        await setProductImagesUseCase.ExecuteAsync(productId, setProductImagesDto, cancellationToken);

        return NoContent();
    }
}
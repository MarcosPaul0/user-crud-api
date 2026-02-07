using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.UseCases.DeleteProduct;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product")]
public class DeleteProductController(IDeleteProductUseCase deleteProductUseCase) : ControllerBase
{
    [Authorize]
    [HttpDelete("{productId:guid}")]
    public async Task<ActionResult> HandleAsync(Guid productId, CancellationToken cancellationToken)
    {
        await deleteProductUseCase.ExecuteAsync(productId, cancellationToken);
        
        return NoContent();
    }
}
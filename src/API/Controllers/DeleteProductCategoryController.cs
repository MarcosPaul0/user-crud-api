using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.UseCases.DeleteProductCategory;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product")]
public class DeleteProductCategoryController(IDeleteProductCategoryUseCase deleteProductCategoryUseCase) : ControllerBase
{
    [Authorize]
    [HttpDelete("{productCategoryId:guid}")]
    public async Task<ActionResult> HandleAsync(Guid productCategoryId, CancellationToken cancellationToken)
    {
        await deleteProductCategoryUseCase.ExecuteAsync(productCategoryId, cancellationToken);
        
        return NoContent();
    }
}
using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.UseCases.ListProductCategory;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class ListProductCategoryController(IListProductCategoryUseCase listProductCategoryUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
        var result = await listProductCategoryUseCase.ExecuteAsync(cancellationToken);

        return Ok();
    }
}
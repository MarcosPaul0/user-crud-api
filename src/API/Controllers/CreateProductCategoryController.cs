using Microsoft.AspNetCore.Mvc;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.CreateProductCategory;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class CreateProductCategoryController(ICreateProductCategoryUseCase createProductCategoryUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] CreateProductCategoryDto createProductCategoryDto,
        CancellationToken cancellationToken)
    {
        await createProductCategoryUseCase.ExecuteAsync(createProductCategoryDto, cancellationToken);

        return Created();
    }
}
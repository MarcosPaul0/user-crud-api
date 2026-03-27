using AutoriaStore.Application.UseCases.ListProductCategory;
using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Dtos;
using UserCrud.API.Presenters;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class ListProductCategoryController(IListProductCategoryUseCase listProductCategoryUseCase) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginationResponseDto<ProductCategoryResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
        var (productCategories, count) = await listProductCategoryUseCase.ExecuteAsync(cancellationToken);

        return Ok(ProductCategoryPresenter.ToHttp(productCategories, count));
    }
}
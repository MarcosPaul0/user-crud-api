using AutoriaStore.Application.UseCases.ListProductCategoryForAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Dtos;
using UserCrud.API.Presenters;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class ListProductCategoryForAdminController(IListProductCategoryForAdminUseCase listProductCategoryForAdminUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet("list/for-admin")]
    [ProducesResponseType(typeof(PaginationResponseDto<ProductCategoryResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
        var (productCategories, count) = await listProductCategoryForAdminUseCase.ExecuteAsync(cancellationToken);

        return Ok(ProductCategoryPresenter.ToHttp(productCategories, count));
    }
}
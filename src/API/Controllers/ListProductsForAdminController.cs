using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Dtos;
using UserCrud.API.Presenters;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.ListProductsForAdmin;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class ListProductsForAdminController(IListProductsForAdminUseCase listProductsForAdminUseCase) : ControllerBase
{
    [Authorize]
    [HttpPost("list/for-admin")]
    [ProducesResponseType(typeof(PaginationResponseDto<ProductForAdminResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(
        [FromBody] ListProductsByAdminDto listProductsByAdminDto,
        CancellationToken cancellationToken)
    {
        var (products, count) = await listProductsForAdminUseCase.ExecuteAsync(listProductsByAdminDto, cancellationToken);

        return Ok(ProductForAdminPresenter.ToHttp(products, count, listProductsByAdminDto.Page, listProductsByAdminDto.ItemsPerPage));
    }
}
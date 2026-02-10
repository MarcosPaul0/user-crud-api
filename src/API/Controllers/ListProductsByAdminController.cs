using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Presenters;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.ListProductsByAdmin;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product")]
public class ListProductsByAdminController(IListProductsByAdminUseCase listProductsByAdminUseCase) : ControllerBase
{
    [Authorize]
    [HttpPost("list/by-admin")]
    public async Task<ActionResult> HandleAsync(
        [FromBody] ListProductsByAdminDto listProductsByAdminDto,
        CancellationToken cancellationToken)
    {
        var (products, count) = await listProductsByAdminUseCase.ExecuteAsync(listProductsByAdminDto, cancellationToken);

        return Ok(ProductByAdminPresenter.ToHttp(products, count, listProductsByAdminDto.Page, listProductsByAdminDto.ItemsPerPage));
    }
}
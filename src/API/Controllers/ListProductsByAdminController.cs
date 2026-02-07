using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Presenters;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.ListProductsByAdmin;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product/by-admin")]
public class ListProductsByAdminController(IListProductsByAdminUseCase listProductsByAdminUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> HandleAsync(
        [FromBody] ListProductByAdminDto listProductByAdminDto,
        CancellationToken cancellationToken)
    {
        var (products, count) = await listProductsByAdminUseCase.ExecuteAsync(listProductByAdminDto, cancellationToken);

        return Ok(ProductByAdminPresenter.ToHttp(products, count, listProductByAdminDto.Page, listProductByAdminDto.ItemsPerPage));
    }
}
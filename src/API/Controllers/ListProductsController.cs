using Microsoft.AspNetCore.Mvc;
using UserCrud.API.Presenters;
using UserCrud.Application.Dtos;
using UserCrud.Application.UseCases.ListProducts;

namespace UserCrud.API.Controllers;

[ApiController]
[Route("api/product")]
public class ListProductsController(IListProductsUseCase listProductsUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> HandleAsync(
        [FromBody] ListProductDto listProductDto,
        CancellationToken cancellationToken)
    {
        var (products, count) = await listProductsUseCase.ExecuteAsync(listProductDto, cancellationToken);

        return Ok(ProductPresenter.ToHttp(products, count, listProductDto.Page, listProductDto.ItemsPerPage));
    }
}
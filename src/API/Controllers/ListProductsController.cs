using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using AutoriaStore.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoriaStore.Application.UseCases.ListProducts;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class ListProductsController(IListProductsUseCase listProductsUseCase) : ControllerBase
{
    [HttpPost("list")]
    [ProducesResponseType(typeof(PaginationResponseDto<ProductListResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(
        [FromBody] ListProductsDto listProductsDto,
        CancellationToken cancellationToken)
    {
        var (products, count) = await listProductsUseCase.ExecuteAsync(listProductsDto, cancellationToken);

        return Ok(ProductPresenter.ToHttp(products, count, listProductsDto.Page, listProductsDto.ItemsPerPage));
    }
}
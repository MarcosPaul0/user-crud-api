// <copyright file="ListProductsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.ListProducts;
using Microsoft.AspNetCore.Mvc;

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

        return this.Ok(ProductPresenter.ToHttp(products, count, listProductsDto.Page, listProductsDto.ItemsPerPage));
    }
}
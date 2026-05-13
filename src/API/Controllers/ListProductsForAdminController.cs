// <copyright file="ListProductsForAdminController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.ListProductsForAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        return this.Ok(ProductForAdminPresenter.ToHttp(products, count, listProductsByAdminDto.Page, listProductsByAdminDto.ItemsPerPage));
    }
}
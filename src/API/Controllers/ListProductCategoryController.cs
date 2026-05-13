// <copyright file="ListProductCategoryController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using AutoriaStore.Application.UseCases.ListProductCategory;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class ListProductCategoryController(IListProductCategoryUseCase listProductCategoryUseCase) : ControllerBase
{
    [HttpGet("list")]
    [ProducesResponseType(typeof(PaginationResponseDto<ProductCategoryResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
    {
        var (productCategories, count) = await listProductCategoryUseCase.ExecuteAsync(cancellationToken);

        return this.Ok(ProductCategoryPresenter.ToHttp(productCategories, count));
    }
}
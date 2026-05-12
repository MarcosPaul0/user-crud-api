// <copyright file="FindProductCategoryByIdController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using AutoriaStore.Application.UseCases.FindProductCategoryById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class FindProductCategoryByIdController(IFindProductCategoryByIdUseCase findProductCategoryByIdUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet("{productCategoryId:guid}")]
    [ProducesResponseType(typeof(ProductCategoryResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productCategoryId,
        CancellationToken cancellationToken)
    {
        var productCategory = await findProductCategoryByIdUseCase.ExecuteAsync(productCategoryId, cancellationToken);

        return this.Ok(ProductCategoryPresenter.ToHttp(productCategory));
    }
}
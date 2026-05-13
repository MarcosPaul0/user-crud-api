// <copyright file="FindProductByIdController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using AutoriaStore.Application.UseCases.FindProductById;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class FindProductByIdController(IFindProductByIdUseCase findProductByIdUseCase) : ControllerBase
{
    [HttpGet("{productId:guid}")]
    [ProducesResponseType(typeof(ProductByIdResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var product = await findProductByIdUseCase.ExecuteAsync(productId, cancellationToken);

        return this.Ok(ProductPresenter.ToHttp(product));
    }
}
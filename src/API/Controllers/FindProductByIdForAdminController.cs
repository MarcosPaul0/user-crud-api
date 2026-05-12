// <copyright file="FindProductByIdForAdminController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.API.Dtos;
using AutoriaStore.API.Presenters;
using AutoriaStore.Application.UseCases.FindProductById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class FindProductByIdForAdminController(IFindProductByIdUseCase findProductByIdUseCase) : ControllerBase
{
    [Authorize]
    [HttpGet("for-admin/{productId:guid}")]
    [ProducesResponseType(typeof(ProductForAdminResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var product = await findProductByIdUseCase.ExecuteAsync(productId, cancellationToken);

        return this.Ok(ProductForAdminPresenter.ToHttp(product));
    }
}
// <copyright file="UpdateProductController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.UpdateProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class UpdateProductController(IUpdateProductUseCase updateProductUseCase) : ControllerBase
{
    [Authorize]
    [HttpPatch("{productId:guid}")]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        [FromBody] UpdateProductDto updateProductDto,
        CancellationToken cancellationToken)
    {
        await updateProductUseCase.ExecuteAsync(productId, updateProductDto, cancellationToken);

        return this.NoContent();
    }
}
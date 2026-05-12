// <copyright file="UpdateProductCategoryController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.UpdateProductCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class UpdateProductCategoryController(IUpdateProductCategoryUseCase updateProductCategoryUseCase) : ControllerBase
{
    [Authorize]
    [HttpPatch("{productCategoryId:guid}")]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productCategoryId,
        [FromBody] UpdateProductCategoryDto updateProductCategoryDto,
        CancellationToken cancellationToken)
    {
        await updateProductCategoryUseCase.ExecuteAsync(productCategoryId, updateProductCategoryDto, cancellationToken);

        return this.NoContent();
    }
}
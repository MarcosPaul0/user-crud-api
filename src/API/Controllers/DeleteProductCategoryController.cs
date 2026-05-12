// <copyright file="DeleteProductCategoryController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.UseCases.DeleteProductCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class DeleteProductCategoryController(IDeleteProductCategoryUseCase deleteProductCategoryUseCase) : ControllerBase
{
    [Authorize]
    [HttpDelete("{productCategoryId:guid}")]
    public async Task<ActionResult> HandleAsync(Guid productCategoryId, CancellationToken cancellationToken)
    {
        await deleteProductCategoryUseCase.ExecuteAsync(productCategoryId, cancellationToken);

        return this.NoContent();
    }
}
// <copyright file="DeleteProductController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.UseCases.DeleteProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class DeleteProductController(IDeleteProductUseCase deleteProductUseCase) : ControllerBase
{
    [Authorize]
    [HttpDelete("{productId:guid}")]
    public async Task<ActionResult> HandleAsync(Guid productId, CancellationToken cancellationToken)
    {
        await deleteProductUseCase.ExecuteAsync(productId, cancellationToken);

        return this.NoContent();
    }
}
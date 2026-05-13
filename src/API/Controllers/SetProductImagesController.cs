// <copyright file="SetProductImagesController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.SetProductImages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class SetProductImagesController(ISetProductImagesUseCase setProductImagesUseCase) : ControllerBase
{
    private const int ProductImageMaxSize = 12_582_912;

    [Authorize]
    [HttpPatch("images/{productId:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(ProductImageMaxSize)]
    [RequestFormLimits(MultipartBodyLengthLimit = ProductImageMaxSize)]
    public async Task<ActionResult> HandleAsync(
        [FromRoute] Guid productId,
        [FromForm] SetProductImagesDto setProductImagesDto,
        CancellationToken cancellationToken)
    {
        await setProductImagesUseCase.ExecuteAsync(productId, setProductImagesDto, cancellationToken);

        return this.NoContent();
    }
}
// <copyright file="CreateProductController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.CreateProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product")]
public class CreateProductController(ICreateProductUseCase createProductUseCase) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] CreateProductDto createProductDto,
        CancellationToken cancellationToken)
    {
        await createProductUseCase.ExecuteAsync(createProductDto, cancellationToken);

        return this.Created();
    }
}
// <copyright file="CreateProductCategoryController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.CreateProductCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/product-category")]
public class CreateProductCategoryController(ICreateProductCategoryUseCase createProductCategoryUseCase) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] CreateProductCategoryDto createProductCategoryDto,
        CancellationToken cancellationToken)
    {
        await createProductCategoryUseCase.ExecuteAsync(createProductCategoryDto, cancellationToken);

        return this.Created();
    }
}
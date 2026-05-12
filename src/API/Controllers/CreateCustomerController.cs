// <copyright file="CreateCustomerController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/user")]
public class CreateCustomerController(ICreateCustomerUseCase createCustomerUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] CreateUserDto createUserDto,
        CancellationToken cancellationToken)
    {
        await createCustomerUseCase.ExecuteAsync(createUserDto, cancellationToken);

        return this.Created();
    }
}
// <copyright file="CreateOrderController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.CreateOrder;
using AutoriaStore.Application.UseCases.FindOrderById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/order")]
public sealed class CreateOrderController(
    ICreateOrderUseCase createOrderUseCase,
    IFindOrderByIdUseCase findOrderByIdUseCase) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CreateOrderResultDto>> HandleAsync(
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey,
        [FromBody] CreateOrderDto createOrderDto,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new BadRequestException(ExceptionMessages.IDEMPOTENCYKEYREQUIRED);
        }

        var endpoint = $"{this.HttpContext.Request.Method}:{this.HttpContext.Request.Path}";
        var result = await createOrderUseCase.ExecuteAsync(createOrderDto, idempotencyKey, endpoint, cancellationToken);

        return this.Ok(result);
    }

    [Authorize]
    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<OrderDetailsDto>> FindByIdAsync(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await findOrderByIdUseCase.ExecuteAsync(orderId, cancellationToken);

        return this.Ok(result);
    }
}

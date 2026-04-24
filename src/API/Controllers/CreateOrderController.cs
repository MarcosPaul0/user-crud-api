using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.CreateOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/order")]
public sealed class CreateOrderController(ICreateOrderUseCase createOrderUseCase) : ControllerBase
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
            throw new BadRequestException(ExceptionMessages.IDEMPOTENCY_KEY_REQUIRED);
        }

        var endpoint = $"{HttpContext.Request.Method}:{HttpContext.Request.Path}";
        await createOrderUseCase.ExecuteAsync(createOrderDto, idempotencyKey, endpoint, cancellationToken);

        return NoContent();
    }
}

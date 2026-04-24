using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.CalculateShipping;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/shipping")]
public sealed class CalculateShippingController(ICalculateShippingUseCase calculateShippingUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> HandleAsync(
        [FromBody] CalculateShippingDto calculateShippingDto,
        CancellationToken cancellationToken)
    {
        var result = await calculateShippingUseCase.ExecuteAsync(calculateShippingDto, cancellationToken);

        return Ok(result);
    }
}
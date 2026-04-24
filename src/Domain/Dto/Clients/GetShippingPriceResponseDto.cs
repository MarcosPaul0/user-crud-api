namespace AutoriaStore.Domain.Dto.Clients;

public record GetShippingPriceResponseDto
{
    public required int PriceInCents { get; init; }
}
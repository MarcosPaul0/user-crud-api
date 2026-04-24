namespace AutoriaStore.Application.Dtos;

public sealed record CreateOrderResultDto
{
    public required Guid OrderId { get; init; }
    public required int TotalPriceInCents { get; init; }
}

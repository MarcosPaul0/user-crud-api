namespace AutoriaStore.Application.Dtos;

public sealed record CreateOrderItemDto
{
    public required Guid ProductId { get; init; }
    public required int Quantity { get; init; }
}

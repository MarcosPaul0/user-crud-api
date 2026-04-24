using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public sealed record CreateOrderDto
{
    [MinLength(1)]
    public required IReadOnlyCollection<CreateOrderItemDto> Items { get; init; }
}

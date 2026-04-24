namespace AutoriaStore.Domain.Dto.Clients;

public record GetDeliveryTimeDto
{
    public required string DestinationPostalCode { get; init; }
}
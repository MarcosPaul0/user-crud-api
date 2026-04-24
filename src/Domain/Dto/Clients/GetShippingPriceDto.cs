namespace AutoriaStore.Domain.Dto.Clients;

public record GetShippingPriceDto
{
    public required string DestinationPostalCode { get; init; }
    public required int DepthInCentimeters { get; init; }
    public required int WidthInCentimeters { get; init; }
    public required int HeightInCentimeters { get; init; }
    public required int WeightInGrams { get; init; }
}
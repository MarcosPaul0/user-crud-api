namespace AutoriaStore.Application.Dtos;

public record CalculateShippingResultDto()
{
    public required int ShippingPriceInCents { get; init; }
    public required DateTime EstimationDeliveryDate { get; init; }
}
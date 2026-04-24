namespace AutoriaStore.Domain.Dto.Clients;

public record GetDeliveryTimeResponseDto
{
    public required DateTime EstimationDeliveryDate  { get; init; }
}
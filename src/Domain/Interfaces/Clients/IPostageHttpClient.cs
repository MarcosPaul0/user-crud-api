using AutoriaStore.Domain.Dto.Clients;

namespace AutoriaStore.Domain.Interfaces.Clients;

public interface IPostageHttpClient
{
    Task<GetShippingPriceResponseDto> GetShippingPriceAsync(
        GetShippingPriceDto getShippingPriceDto,
        CancellationToken cancellationToken = default);
    
    Task<GetDeliveryTimeResponseDto> GetDeliveryTimeAsync(
        GetDeliveryTimeDto getDeliveryTimeDto,
        CancellationToken cancellationToken = default);
}
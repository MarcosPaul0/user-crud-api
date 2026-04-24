using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Dto.Clients;
using AutoriaStore.Domain.Interfaces;
using AutoriaStore.Domain.Interfaces.Clients;
using AutoriaStore.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace AutoriaStore.Application.UseCases.CalculateShipping;

public sealed class CalculateShippingUseCase(
    IMemoryCache memoryCache,
    IPostageHttpClient postageHttpClient, 
    IUnitOfWork unitOfWork) : ICalculateShippingUseCase
{
    public async Task<CalculateShippingResultDto> ExecuteAsync(
        CalculateShippingDto calculateShippingDto, 
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Product.FindByIdAsync(calculateShippingDto.ProductId, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_NOT_FOUND);
        }

        var hasShippingResult = memoryCache.TryGetValue<CalculateShippingResultDto>($"shipping:{calculateShippingDto.DestinationPostalCode}",
            out var shippingResultInCache);
            
        if (hasShippingResult && shippingResultInCache is not null)
        {
            return shippingResultInCache;
        }
        
        var getDeliveryTimeDto = new GetDeliveryTimeDto()
        {
            DestinationPostalCode = calculateShippingDto.DestinationPostalCode,
        };
        
        var deliveryResult = await postageHttpClient.GetDeliveryTimeAsync(getDeliveryTimeDto, cancellationToken);
        
        var getShippingPriceDto = new GetShippingPriceDto()
        {
            WeightInGrams = product.WeightInGrams,
            DepthInCentimeters = product.DepthInCentimeters,
            WidthInCentimeters =  product.WidthInCentimeters,
            HeightInCentimeters = product.HeightInCentimeters,
            DestinationPostalCode = calculateShippingDto.DestinationPostalCode,
        };

        var shippingPrice = await postageHttpClient.GetShippingPriceAsync(getShippingPriceDto, cancellationToken);

        var shippingResult = new CalculateShippingResultDto()
        {
            ShippingPriceInCents = shippingPrice.PriceInCents,
            EstimationDeliveryDate = deliveryResult.EstimationDeliveryDate
        };
        
        memoryCache.Set($"shipping:{calculateShippingDto.DestinationPostalCode}", shippingResult, TimeSpan.FromDays(7));
        
        return shippingResult;
    }
}
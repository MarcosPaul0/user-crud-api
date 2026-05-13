// <copyright file="IPostageHttpClient.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
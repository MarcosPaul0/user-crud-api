// <copyright file="CalculateShippingResultDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Dtos;

public record CalculateShippingResultDto()
{
    required public int ShippingPriceInCents { get; init; }
    required public DateTime EstimationDeliveryDate { get; init; }
}
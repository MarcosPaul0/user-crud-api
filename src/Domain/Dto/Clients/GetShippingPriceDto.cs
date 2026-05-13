// <copyright file="GetShippingPriceDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Clients;

public record GetShippingPriceDto
{
    required public string DestinationPostalCode { get; init; }
    required public int DepthInCentimeters { get; init; }
    required public int WidthInCentimeters { get; init; }
    required public int HeightInCentimeters { get; init; }
    required public int WeightInGrams { get; init; }
}
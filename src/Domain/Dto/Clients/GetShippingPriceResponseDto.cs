// <copyright file="GetShippingPriceResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Clients;

public record GetShippingPriceResponseDto
{
    required public int PriceInCents { get; init; }
}
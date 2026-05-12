// <copyright file="GetDeliveryTimeDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Clients;

public record GetDeliveryTimeDto
{
    required public string DestinationPostalCode { get; init; }
}
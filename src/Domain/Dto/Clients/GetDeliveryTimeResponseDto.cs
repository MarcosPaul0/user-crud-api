// <copyright file="GetDeliveryTimeResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Clients;

public record GetDeliveryTimeResponseDto
{
    required public DateTime EstimationDeliveryDate { get; init; }
}
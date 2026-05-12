// <copyright file="CreateOrderResultDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Dtos;

public sealed record CreateOrderResultDto
{
    required public Guid OrderId { get; init; }
    required public int TotalPriceInCents { get; init; }
    required public string OrderStatus { get; init; }
    required public string PaymentId { get; init; }
    required public string PaymentStatus { get; init; }
    required public string PixCopyPasteCode { get; init; }
    required public string PixQrCodeBase64 { get; init; }
    required public DateTime PixExpiresAt { get; init; }
}

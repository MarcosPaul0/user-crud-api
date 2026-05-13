// <copyright file="OrderDetailsDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Dtos;

public sealed record OrderDetailsDto
{
    required public Guid OrderId { get; init; }
    required public int TotalPriceInCents { get; init; }
    required public string OrderStatus { get; init; }
    required public string PaymentStatus { get; init; }
    public string? PaymentId { get; init; }
    public string? PaymentMethod { get; init; }
    public string? PaymentProvider { get; init; }
    public string? PixCopyPasteCode { get; init; }
    public string? PixQrCodeBase64 { get; init; }
    public DateTime? PaymentExpiresAt { get; init; }
    public DateTime? PaidAt { get; init; }
    public string? ReceiptUrl { get; init; }
    required public IReadOnlyCollection<OrderItemSummaryDto> Items { get; init; }
}

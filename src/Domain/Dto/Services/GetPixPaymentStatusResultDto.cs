// <copyright file="GetPixPaymentStatusResultDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Services;

public sealed record GetPixPaymentStatusResultDto
{
    required public string PaymentId { get; init; }
    required public string Status { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public string? ReceiptUrl { get; init; }
}

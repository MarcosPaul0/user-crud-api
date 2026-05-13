// <copyright file="CreatePixPaymentResultDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Services;

public sealed record CreatePixPaymentResultDto
{
    required public string PaymentId { get; init; }
    required public string Status { get; init; }
    required public string BrCode { get; init; }
    required public string BrCodeBase64 { get; init; }
    required public DateTime ExpiresAt { get; init; }
}

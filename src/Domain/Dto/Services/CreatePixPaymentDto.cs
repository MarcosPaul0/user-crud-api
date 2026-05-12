// <copyright file="CreatePixPaymentDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Services;

public sealed record CreatePixPaymentDto
{
    required public int AmountInCents { get; init; }
    required public string Description { get; init; }
    required public string ExternalId { get; init; }
    required public IReadOnlyDictionary<string, string> Metadata { get; init; }
    required public int ExpiresInSeconds { get; init; }
    public CreatePixPaymentCustomerDto? Customer { get; init; }
}

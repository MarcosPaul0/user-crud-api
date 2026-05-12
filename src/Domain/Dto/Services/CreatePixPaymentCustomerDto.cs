// <copyright file="CreatePixPaymentCustomerDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Services;

public sealed record CreatePixPaymentCustomerDto
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? TaxId { get; init; }
    public string? Cellphone { get; init; }
}

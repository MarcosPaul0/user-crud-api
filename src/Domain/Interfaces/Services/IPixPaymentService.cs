// <copyright file="IPixPaymentService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Dto.Services;

namespace AutoriaStore.Domain.Interfaces.Services;

public interface IPixPaymentService
{
    Task<CreatePixPaymentResultDto> CreateAsync(
        CreatePixPaymentDto createPixPaymentDto,
        CancellationToken cancellationToken);

    Task<GetPixPaymentStatusResultDto> GetStatusAsync(
        string paymentId,
        CancellationToken cancellationToken);
}

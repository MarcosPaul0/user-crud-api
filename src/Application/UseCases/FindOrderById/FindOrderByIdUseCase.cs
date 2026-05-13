// <copyright file="FindOrderByIdUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.Application.UseCases.FindOrderById;

public sealed class FindOrderByIdUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IPixPaymentService pixPaymentService,
    IUnitOfWork unitOfWork) : IFindOrderByIdUseCase
{
    public async Task<OrderDetailsDto> ExecuteAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var authenticatedUserId = authenticatedUserService.GetUserId();

        if (authenticatedUserId == null || authenticatedUserId == Guid.Empty)
        {
            throw new UnauthorizeException(ExceptionMessages.USER_NOT_AUTHENTICATED);
        }

        var order = await unitOfWork.Order.FindByUserIdAndOrderIdAsync(
            authenticatedUserId.Value,
            orderId,
            cancellationToken);

        if (order is null)
        {
            throw new NotFoundException(ExceptionMessages.ORDER_NOT_FOUND);
        }

        await this.SynchronizePendingPaymentAsync(order, cancellationToken);

        return Map(order);
    }

    private static OrderDetailsDto Map(Order order)
    {
        return new OrderDetailsDto
        {
            OrderId = order.Id,
            TotalPriceInCents = order.TotalPriceInCents,
            OrderStatus = order.Status.ToString(),
            PaymentStatus = order.PaymentStatus.ToString(),
            PaymentId = order.PaymentId,
            PaymentMethod = order.PaymentMethod,
            PaymentProvider = order.PaymentProvider,
            PixCopyPasteCode = order.PixCopyPasteCode,
            PixQrCodeBase64 = order.PixQrCodeBase64,
            PaymentExpiresAt = order.PaymentExpiresAt,
            PaidAt = order.PaidAt,
            ReceiptUrl = order.ReceiptUrl,
            Items = order.ProductOrders.Select(item => new OrderItemSummaryDto
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPriceInCents = item.UnitPriceInCents,
                TotalPriceInCents = item.TotalPriceInCents,
            }).ToList(),
        };
    }

    private async Task SynchronizePendingPaymentAsync(Order order, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(order.PaymentId) || order.PaymentStatus != OrderPaymentStatus.Pending)
        {
            return;
        }

        var paymentStatus = await pixPaymentService.GetStatusAsync(order.PaymentId, cancellationToken);
        var mappedStatus = OrderPaymentStatusExtensions.FromProviderStatus(paymentStatus.Status);

        if (mappedStatus == order.PaymentStatus &&
            paymentStatus.ExpiresAt == order.PaymentExpiresAt &&
            paymentStatus.ReceiptUrl == order.ReceiptUrl)
        {
            return;
        }

        order.ApplyPaymentStatus(
            mappedStatus,
            paymentStatus.ExpiresAt ?? DateTime.UtcNow,
            paymentStatus.ReceiptUrl,
            paymentStatus.ExpiresAt);

        await unitOfWork.SaveChangesAsync();
    }
}

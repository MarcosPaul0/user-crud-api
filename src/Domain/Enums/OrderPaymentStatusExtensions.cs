// <copyright file="OrderPaymentStatusExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Enums;

public static class OrderPaymentStatusExtensions
{
    public static OrderPaymentStatus FromProviderStatus(string providerStatus)
    {
        return providerStatus.Trim().ToUpperInvariant() switch
        {
            "PENDING" => OrderPaymentStatus.Pending,
            "PAID" => OrderPaymentStatus.Paid,
            "EXPIRED" => OrderPaymentStatus.Expired,
            "CANCELLED" => OrderPaymentStatus.Cancelled,
            "REFUNDED" => OrderPaymentStatus.Refunded,
            "DISPUTED" => OrderPaymentStatus.Disputed,
            "LOST" => OrderPaymentStatus.Lost,
            _ => throw new InvalidOperationException($"Unsupported payment status '{providerStatus}'.")
        };
    }

    public static OrderStatus ToOrderStatus(this OrderPaymentStatus paymentStatus)
    {
        return paymentStatus switch
        {
            OrderPaymentStatus.Pending => OrderStatus.AwaitingPayment,
            OrderPaymentStatus.Paid => OrderStatus.Paid,
            OrderPaymentStatus.Expired => OrderStatus.PaymentExpired,
            OrderPaymentStatus.Cancelled => OrderStatus.PaymentCancelled,
            OrderPaymentStatus.Refunded => OrderStatus.Refunded,
            OrderPaymentStatus.Disputed => OrderStatus.Disputed,
            OrderPaymentStatus.Lost => OrderStatus.Lost,
            _ => throw new InvalidOperationException($"Unsupported payment status '{paymentStatus}'.")
        };
    }
}

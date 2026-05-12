// <copyright file="Order.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Enums;

namespace AutoriaStore.Domain.Entities;

public class Order : Entity
{
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public int TotalPriceInCents { get; set; }

    public OrderStatus Status { get; private set; }

    public OrderPaymentStatus PaymentStatus { get; private set; }

    public string PaymentProvider { get; private set; } = string.Empty;

    public string PaymentMethod { get; private set; } = string.Empty;

    public string? PaymentId { get; private set; }

    public string? PaymentExternalId { get; private set; }

    public string? PixCopyPasteCode { get; private set; }

    public string? PixQrCodeBase64 { get; private set; }

    public DateTime? PaymentExpiresAt { get; private set; }

    public DateTime? PaidAt { get; private set; }

    public DateTime? RefundedAt { get; private set; }

    public DateTime? DisputedAt { get; private set; }

    public DateTime? CancelledAt { get; private set; }

    public string? ReceiptUrl { get; private set; }

    public List<OrderProduct> ProductOrders { get; set; } =[];

    public void StartPixPayment(
        string paymentProvider,
        string paymentMethod,
        string paymentId,
        string paymentExternalId,
        string paymentStatus,
        string pixCopyPasteCode,
        string pixQrCodeBase64,
        DateTime paymentExpiresAt)
    {
        this.PaymentProvider = paymentProvider;
        this.PaymentMethod = paymentMethod;
        this.PaymentId = paymentId;
        this.PaymentExternalId = paymentExternalId;
        this.PixCopyPasteCode = pixCopyPasteCode;
        this.PixQrCodeBase64 = pixQrCodeBase64;

        this.ApplyPaymentStatus(
            OrderPaymentStatusExtensions.FromProviderStatus(paymentStatus),
            paymentExpiresAt,
            null,
            paymentExpiresAt);
    }

    public void ApplyPaymentStatus(
        OrderPaymentStatus paymentStatus,
        DateTime? occurredAt = null,
        string? receiptUrl = null,
        DateTime? paymentExpiresAt = null)
    {
        this.PaymentStatus = paymentStatus;

        if (paymentExpiresAt.HasValue)
        {
            this.PaymentExpiresAt = paymentExpiresAt.Value;
        }

        if (!string.IsNullOrWhiteSpace(receiptUrl))
        {
            this.ReceiptUrl = receiptUrl;
        }

        this.Status = paymentStatus.ToOrderStatus();

        switch (paymentStatus)
        {
            case OrderPaymentStatus.Paid:
                this.PaidAt ??= occurredAt ?? DateTime.UtcNow;
                break;
            case OrderPaymentStatus.Refunded:
                this.RefundedAt ??= occurredAt ?? DateTime.UtcNow;
                break;
            case OrderPaymentStatus.Disputed:
            case OrderPaymentStatus.Lost:
                this.DisputedAt ??= occurredAt ?? DateTime.UtcNow;
                break;
            case OrderPaymentStatus.Cancelled:
            case OrderPaymentStatus.Expired:
                this.CancelledAt ??= occurredAt ?? DateTime.UtcNow;
                break;
        }

        this.UpdatedAt = DateTime.UtcNow;
    }
}

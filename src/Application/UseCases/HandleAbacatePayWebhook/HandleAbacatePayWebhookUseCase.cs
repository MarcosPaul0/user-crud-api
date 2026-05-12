// <copyright file="HandleAbacatePayWebhookUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json;
using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.Application.UseCases.HandleAbacatePayWebhook;

public sealed class HandleAbacatePayWebhookUseCase(
    IAbacatePayWebhookSignatureService abacatePayWebhookSignatureService,
    IEnvironmentVariablesService environmentVariablesService,
    IUnitOfWork unitOfWork) : IHandleAbacatePayWebhookUseCase
{
    public async Task ExecuteAsync(
        HandleAbacatePayWebhookDto handleAbacatePayWebhookDto,
        CancellationToken cancellationToken)
    {
        this.ValidateWebhook(handleAbacatePayWebhookDto);

        using var document = JsonDocument.Parse(handleAbacatePayWebhookDto.RawBody);
        var root = document.RootElement;
        var eventName = root.GetProperty("event").GetString();

        if (string.IsNullOrWhiteSpace(eventName) || !eventName.StartsWith("transparent.", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var transparent = root.GetProperty("data").GetProperty("transparent");
        var paymentId = transparent.GetProperty("id").GetString();

        if (string.IsNullOrWhiteSpace(paymentId))
        {
            return;
        }

        var order = await unitOfWork.Order.FindByPaymentIdAsync(paymentId, cancellationToken);

        if (order is null)
        {
            return;
        }

        ApplyEvent(order, eventName, transparent);
        await unitOfWork.SaveChangesAsync();
    }

    private static void ApplyEvent(Order order, string eventName, JsonElement transparent)
    {
        var paymentStatus = eventName switch
        {
            "transparent.completed" => OrderPaymentStatus.Paid,
            "transparent.refunded" => OrderPaymentStatus.Refunded,
            "transparent.disputed" => OrderPaymentStatus.Disputed,
            "transparent.lost" => OrderPaymentStatus.Lost,
            _ => OrderPaymentStatusExtensions.FromProviderStatus(transparent.GetProperty("status").GetString() ?? "PENDING")
        };

        var updatedAt = transparent.TryGetProperty("updatedAt", out var updatedAtElement) &&
                        updatedAtElement.TryGetDateTime(out var occurredAt)
            ? occurredAt
            : DateTime.UtcNow;

        var receiptUrl = transparent.TryGetProperty("receiptUrl", out var receiptUrlElement)
            ? receiptUrlElement.GetString()
            : null;

        order.ApplyPaymentStatus(paymentStatus, updatedAt, receiptUrl);
    }

    private void ValidateWebhook(HandleAbacatePayWebhookDto handleAbacatePayWebhookDto)
    {
        if (!string.Equals(
                handleAbacatePayWebhookDto.WebhookSecret,
                environmentVariablesService.AbacatePayWebhookSecret,
                StringComparison.Ordinal))
        {
            throw new UnauthorizeException(ExceptionMessages.ABACATEPAYWEBHOOKUNAUTHORIZED);
        }

        if (!abacatePayWebhookSignatureService.IsValid(
                handleAbacatePayWebhookDto.RawBody,
                handleAbacatePayWebhookDto.Signature))
        {
            throw new UnauthorizeException(ExceptionMessages.ABACATEPAYWEBHOOKUNAUTHORIZED);
        }
    }
}

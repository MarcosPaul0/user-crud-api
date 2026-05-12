// <copyright file="CreateOrderUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json;
using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace AutoriaStore.Application.UseCases.CreateOrder;

public sealed class CreateOrderUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IIdempotencyService idempotencyService,
    IPixPaymentService pixPaymentService,
    IUnitOfWork unitOfWork) : ICreateOrderUseCase
{
    public async Task<CreateOrderResultDto> ExecuteAsync(
        CreateOrderDto createOrderDto,
        string idempotencyKey,
        string endpoint,
        CancellationToken cancellationToken)
    {
        var authenticatedUserId = authenticatedUserService.GetUserId();

        if (authenticatedUserId == null || authenticatedUserId == Guid.Empty)
        {
            throw new UnauthorizeException(ExceptionMessages.USERNOTAUTHENTICATED);
        }

        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new BadRequestException(ExceptionMessages.IDEMPOTENCYKEYREQUIRED);
        }

        if (createOrderDto.Items.Count == 0)
        {
            throw new ConflictException(ExceptionMessages.ORDERITEMSREQUIRED);
        }

        var existingIdempotencyKey = await idempotencyService.GetIdempotencyKeyAsync(
            authenticatedUserId.Value,
            idempotencyKey,
            endpoint,
            cancellationToken);

        if (existingIdempotencyKey is not null)
        {
            var idempotencyKeyIsExpired = await idempotencyService.RemoveIdempotencyKeyIfExpiredAsync(
                existingIdempotencyKey,
                cancellationToken);

            if (!idempotencyKeyIsExpired)
            {
                return DeserializeResponse(existingIdempotencyKey.ResponseBody);
            }
        }

        return await this.CreateOrderAsync(
            createOrderDto,
            authenticatedUserId.Value,
            idempotencyKey,
            endpoint,
            cancellationToken);
    }

    private static string BuildPixDescription(Guid orderId)
    {
        return $"Pedido {orderId.ToString("N")[..8]}";
    }

    private static CreateOrderResultDto DeserializeResponse(string? responseBody)
    {
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            throw new ConflictException("Idempotency key already used without stored response.");
        }

        var response = JsonSerializer.Deserialize<CreateOrderResultDto>(
            responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

        if (response is null)
        {
            throw new InvalidOperationException("Stored idempotency response is invalid.");
        }

        return response;
    }

    private async Task<CreateOrderResultDto> CreateOrderAsync(
        CreateOrderDto createOrderDto,
        Guid authenticatedUserId,
        string idempotencyKey,
        string endpoint,
        CancellationToken cancellationToken)
    {
        var orderProducts = new List<OrderProduct>(createOrderDto.Items.Count);
        var totalPriceInCents = 0;
        var customer = await unitOfWork.User.FindByIdAsync(authenticatedUserId, cancellationToken);

        if (customer is null)
        {
            throw new NotFoundException(ExceptionMessages.USERNOTFOUND);
        }

        foreach (var item in createOrderDto.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new ConflictException(ExceptionMessages.ORDERITEMQUANTITYINVALID);
            }

            var product = await unitOfWork.Product.FindByIdAsync(item.ProductId, cancellationToken);

            if (product is not { IsActive: true })
            {
                throw new NotFoundException(ExceptionMessages.PRODUCTNOTFOUND);
            }

            totalPriceInCents += product.PriceInCents * item.Quantity;

            orderProducts.Add(new OrderProduct
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                UnitPriceInCents = product.PriceInCents,
                TotalPriceInCents = product.PriceInCents * item.Quantity,
                CreatedAt = DateTime.UtcNow,
            });
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = authenticatedUserId,
            TotalPriceInCents = totalPriceInCents,
            ProductOrders = orderProducts,
            CreatedAt = DateTime.UtcNow,
        };

        var payment = await pixPaymentService.CreateAsync(
            new CreatePixPaymentDto
            {
                AmountInCents = totalPriceInCents,
                Description = BuildPixDescription(order.Id),
                ExternalId = order.Id.ToString(),
                ExpiresInSeconds = 3600,
                Customer = new CreatePixPaymentCustomerDto
                {
                    Name = customer.Name,
                    Email = customer.Email,
                },
                Metadata = new Dictionary<string, string>
                {
                    ["orderId"] = order.Id.ToString(),
                    ["userId"] = authenticatedUserId.ToString(),
                },
            }, cancellationToken);

        order.StartPixPayment(
            "ABACATE_PAY",
            "PIX",
            payment.PaymentId,
            order.Id.ToString(),
            payment.Status,
            payment.BrCode,
            payment.BrCodeBase64,
            payment.ExpiresAt);

        var result = new CreateOrderResultDto
        {
            OrderId = order.Id,
            TotalPriceInCents = totalPriceInCents,
            OrderStatus = order.Status.ToString(),
            PaymentId = payment.PaymentId,
            PaymentStatus = order.PaymentStatus.ToString(),
            PixCopyPasteCode = payment.BrCode,
            PixQrCodeBase64 = payment.BrCodeBase64,
            PixExpiresAt = payment.ExpiresAt,
        };

        var newIdempotencyKey = new CreateIdempotencyKeyDto
        {
            ResponseObject = result,
            RequestObject = createOrderDto,
            StatusCode = StatusCodes.Status200OK,
            AuthenticatedUserId = authenticatedUserId,
            Endpoint = endpoint,
            IdempotencyKey = idempotencyKey,
        };

        await idempotencyService.CreateIdempotencyKeyAsync(newIdempotencyKey, cancellationToken);
        await unitOfWork.Order.CreateAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync();

        return result;
    }
}

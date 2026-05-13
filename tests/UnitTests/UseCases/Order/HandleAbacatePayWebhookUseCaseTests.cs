// <copyright file="HandleAbacatePayWebhookUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json;
using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.HandleAbacatePayWebhook;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.UnitTests.UseCases.Order;

public class HandleAbacatePayWebhookUseCaseTests
{
    private readonly Mock<IAbacatePayWebhookSignatureService> signatureServiceMock = new ();
    private readonly Mock<IEnvironmentVariablesService> environmentVariablesServiceMock = new ();
    private readonly Mock<IUnitOfWork> unitOfWorkMock = new ();
    private readonly Mock<IOrderRepository> orderRepositoryMock = new ();
    private readonly HandleAbacatePayWebhookUseCase sut;

    public HandleAbacatePayWebhookUseCaseTests()
    {
        this.unitOfWorkMock.Setup(unitOfWork => unitOfWork.Order).Returns(this.orderRepositoryMock.Object);
        this.environmentVariablesServiceMock.SetupGet(service => service.AbacatePayWebhookSecret).Returns("secret-123");

        this.sut = new HandleAbacatePayWebhookUseCase(
            this.signatureServiceMock.Object,
            this.environmentVariablesServiceMock.Object,
            this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenWebhookIsUnauthorized_ThrowsUnauthorizeException()
    {
        this.signatureServiceMock.Setup(service => service.IsValid(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        var act = () => this.sut.ExecuteAsync(
            new HandleAbacatePayWebhookDto
            {
                WebhookSecret = "secret-123",
                Signature = "invalid",
                RawBody = "{}",
            }, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizeException>(act);
        Assert.Equal(ExceptionMessages.ABACATE_PAY_WEBHOOK_UNAUTHORIZED, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenTransparentCompletedEventIsReceived_UpdatesOrderStatus()
    {
        var order = new Domain.Entities.Order
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            TotalPriceInCents = 5000,
            CreatedAt = DateTime.UtcNow,
        };
        order.StartPixPayment(
            "ABACATE_PAY",
            "PIX",
            "char_xyz789",
            order.Id.ToString(),
            "PENDING",
            "000201",
            "data:image/png;base64,abc",
            DateTime.UtcNow.AddMinutes(30));

        var payload = JsonSerializer.Serialize(new
        {
            @event = "transparent.completed",
            apiVersion = 2,
            devMode = false,
            data = new
            {
                transparent = new
                {
                    id = "char_xyz789",
                    externalId = order.Id.ToString(),
                    amount = 5000,
                    paidAmount = 5000,
                    status = "PAID",
                    receiptUrl = "https://abacatepay.com/receipt/123",
                    updatedAt = DateTime.UtcNow,
                },
            },
        });

        this.signatureServiceMock.Setup(service => service.IsValid(payload, "sig-123")).Returns(true);
        this.orderRepositoryMock
            .Setup(repository => repository.FindByPaymentIdAsync("char_xyz789", It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        await this.sut.ExecuteAsync(
            new HandleAbacatePayWebhookDto
            {
                WebhookSecret = "secret-123",
                Signature = "sig-123",
                RawBody = payload,
            }, CancellationToken.None);

        Assert.Equal(OrderStatus.Paid, order.Status);
        Assert.Equal(OrderPaymentStatus.Paid, order.PaymentStatus);
        Assert.Equal("https://abacatepay.com/receipt/123", order.ReceiptUrl);
        this.unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
    }
}

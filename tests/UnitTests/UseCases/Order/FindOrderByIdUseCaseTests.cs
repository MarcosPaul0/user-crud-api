// <copyright file="FindOrderByIdUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.FindOrderById;
using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.UnitTests.UseCases.Order;

public class FindOrderByIdUseCaseTests
{
    private readonly Mock<IAuthenticatedUserService> authenticatedUserServiceMock = new ();
    private readonly Mock<IPixPaymentService> pixPaymentServiceMock = new ();
    private readonly Mock<IUnitOfWork> unitOfWorkMock = new ();
    private readonly Mock<IOrderRepository> orderRepositoryMock = new ();
    private readonly FindOrderByIdUseCase sut;

    public FindOrderByIdUseCaseTests()
    {
        this.unitOfWorkMock.Setup(unitOfWork => unitOfWork.Order).Returns(this.orderRepositoryMock.Object);
        this.sut = new FindOrderByIdUseCase(
            this.authenticatedUserServiceMock.Object,
            this.pixPaymentServiceMock.Object,
            this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserIsNotAuthenticated_ThrowsUnauthorizeException()
    {
        this.authenticatedUserServiceMock.Setup(service => service.GetUserId()).Returns((Guid?)null);

        var act = () => this.sut.ExecuteAsync(Guid.NewGuid(), CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizeException>(act);
        Assert.Equal(ExceptionMessages.USERNOTAUTHENTICATED, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOrderIsPending_RefreshesStatusFromGateway()
    {
        var userId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var order = new Domain.Entities.Order
        {
            Id = orderId,
            UserId = userId,
            TotalPriceInCents = 2500,
            ProductOrders =
            [
                new OrderProduct
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    ProductName = "Poster",
                    Quantity = 1,
                    UnitPriceInCents = 2500,
                    TotalPriceInCents = 2500,
                    CreatedAt = DateTime.UtcNow,
                }

            ],
            CreatedAt = DateTime.UtcNow,
        };
        order.StartPixPayment(
            "ABACATE_PAY",
            "PIX",
            "pix_123",
            orderId.ToString(),
            "PENDING",
            "000201",
            "data:image/png;base64,abc",
            DateTime.UtcNow.AddMinutes(30));

        this.authenticatedUserServiceMock.Setup(service => service.GetUserId()).Returns(userId);
        this.orderRepositoryMock
            .Setup(repository => repository.FindByUserIdAndOrderIdAsync(userId, orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        this.pixPaymentServiceMock
            .Setup(service => service.GetStatusAsync("pix_123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetPixPaymentStatusResultDto
            {
                PaymentId = "pix_123",
                Status = "PAID",
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                ReceiptUrl = "https://abacatepay.com/receipt/123",
            });

        var result = await this.sut.ExecuteAsync(orderId, CancellationToken.None);

        Assert.Equal(OrderStatus.Paid.ToString(), result.OrderStatus);
        Assert.Equal(OrderPaymentStatus.Paid.ToString(), result.PaymentStatus);
        Assert.Equal("https://abacatepay.com/receipt/123", result.ReceiptUrl);
        this.unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
    }
}

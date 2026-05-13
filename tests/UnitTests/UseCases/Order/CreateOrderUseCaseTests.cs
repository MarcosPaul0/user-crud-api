// <copyright file="CreateOrderUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json;
using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.CreateOrder;
using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using OrderEntity = AutoriaStore.Domain.Entities.Order;
using ProductEntity = AutoriaStore.Domain.Entities.Product;

namespace AutoriaStore.UnitTests.UseCases.Order;

public class CreateOrderUseCaseTests
{
    private const string Endpoint = "POST:/api/order";

    private readonly Mock<IAuthenticatedUserService> authenticatedUserServiceMock;
    private readonly Mock<IIdempotencyService> idempotencyServiceMock;
    private readonly Mock<IPixPaymentService> pixPaymentServiceMock;
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IOrderRepository> orderRepositoryMock;
    private readonly Mock<IProductRepository> productRepositoryMock;
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly CreateOrderUseCase sut;

    public CreateOrderUseCaseTests()
    {
        this.authenticatedUserServiceMock = new Mock<IAuthenticatedUserService>();
        this.idempotencyServiceMock = new Mock<IIdempotencyService>();
        this.pixPaymentServiceMock = new Mock<IPixPaymentService>();
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.orderRepositoryMock = new Mock<IOrderRepository>();
        this.productRepositoryMock = new Mock<IProductRepository>();
        this.userRepositoryMock = new Mock<IUserRepository>();

        this.unitOfWorkMock.Setup(u => u.Order).Returns(this.orderRepositoryMock.Object);
        this.unitOfWorkMock.Setup(u => u.Product).Returns(this.productRepositoryMock.Object);
        this.unitOfWorkMock.Setup(u => u.User).Returns(this.userRepositoryMock.Object);
        this.orderRepositoryMock
            .Setup(repository => repository.CreateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((OrderEntity order, CancellationToken _) => order);

        this.sut = new CreateOrderUseCase(
            this.authenticatedUserServiceMock.Object,
            this.idempotencyServiceMock.Object,
            this.pixPaymentServiceMock.Object,
            this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserIsNotAuthenticated_ThrowsUnauthorizeException()
    {
        var dto = BuildDto();

        this.authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns((Guid?)null);

        var act = () => this.sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizeException>(act);

        Assert.Equal(ExceptionMessages.USER_NOT_AUTHENTICATED, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenIdempotencyKeyIsMissing_ThrowsBadRequestException()
    {
        var authenticatedUserId = Guid.NewGuid();
        var dto = BuildDto();

        this.authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);

        var act = () => this.sut.ExecuteAsync(dto, " ", Endpoint, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<BadRequestException>(act);

        Assert.Equal(ExceptionMessages.IDEMPOTENCY_KEY_REQUIRED, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenItemsAreEmpty_ThrowsConflictException()
    {
        var authenticatedUserId = Guid.NewGuid();
        var dto = new CreateOrderDto { Items = [] };

        this.authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);

        var act = () => this.sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);

        Assert.Equal(ExceptionMessages.ORDER_ITEMS_REQUIRED, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenIdempotencyKeyExistsAndIsNotExpired_ReturnsStoredResponse()
    {
        var authenticatedUserId = Guid.NewGuid();
        var dto = BuildDto();
        var existingResponse = new CreateOrderResultDto
        {
            OrderId = Guid.NewGuid(),
            TotalPriceInCents = 5300,
            OrderStatus = OrderStatus.AwaitingPayment.ToString(),
            PaymentId = "pix_123",
            PaymentStatus = OrderPaymentStatus.Pending.ToString(),
            PixCopyPasteCode = "000201",
            PixQrCodeBase64 = "data:image/png;base64,abc",
            PixExpiresAt = DateTime.UtcNow.AddHours(1),
        };
        var existingIdempotencyKey = new IdempotencyKey
        {
            Id = Guid.NewGuid(),
            UserId = authenticatedUserId,
            Key = "request-key",
            Endpoint = Endpoint,
            RequestHash = "hash",
            ResponseStatus = StatusCodes.Status200OK,
            ResponseBody = JsonSerializer.Serialize(existingResponse),
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            CreatedAt = DateTime.UtcNow,
        };

        this.authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);
        this.userRepositoryMock
            .Setup(repository => repository.FindByIdAsync(authenticatedUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BuildUser(authenticatedUserId));

        this.idempotencyServiceMock
            .Setup(service => service.GetIdempotencyKeyAsync(authenticatedUserId, "request-key", Endpoint, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingIdempotencyKey);

        this.idempotencyServiceMock
            .Setup(service => service.RemoveIdempotencyKeyIfExpiredAsync(existingIdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await this.sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);

        Assert.Equal(existingResponse.OrderId, result.OrderId);
        Assert.Equal(existingResponse.PaymentId, result.PaymentId);
        this.orderRepositoryMock.Verify(
            repository => repository.CreateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
        this.pixPaymentServiceMock.Verify(
            service => service.CreateAsync(It.IsAny<CreatePixPaymentDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductIsMissing_ThrowsNotFoundException()
    {
        var authenticatedUserId = Guid.NewGuid();
        var dto = BuildDto();

        this.authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);
        this.userRepositoryMock
            .Setup(repository => repository.FindByIdAsync(authenticatedUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BuildUser(authenticatedUserId));

        this.idempotencyServiceMock
            .Setup(service => service.GetIdempotencyKeyAsync(
                authenticatedUserId,
                "request-key",
                Endpoint,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((IdempotencyKey?)null);

        this.productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductEntity?)null);

        var act = () => this.sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

        Assert.Equal(ExceptionMessages.PRODUCT_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenExistingIdempotencyKeyIsExpiredAndRequestIsValid_CreatesOrderPaymentAndIdempotencyRecord()
    {
        var authenticatedUserId = Guid.NewGuid();
        var firstProductId = Guid.NewGuid();
        var secondProductId = Guid.NewGuid();
        var existingIdempotencyKey = new IdempotencyKey
        {
            Id = Guid.NewGuid(),
            UserId = authenticatedUserId,
            Key = "request-key",
            Endpoint = Endpoint,
            RequestHash = "hash",
            ResponseStatus = StatusCodes.Status200OK,
            ResponseBody = "{}",
            ExpiresAt = DateTime.UtcNow.AddHours(-1),
            CreatedAt = DateTime.UtcNow,
        };

        var dto = new CreateOrderDto
        {
            Items =
            [
                new CreateOrderItemDto { ProductId = firstProductId, Quantity = 2 },
                new CreateOrderItemDto { ProductId = secondProductId, Quantity = 1 }
            ],
        };

        this.authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);
        this.userRepositoryMock
            .Setup(repository => repository.FindByIdAsync(authenticatedUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BuildUser(authenticatedUserId));

        this.idempotencyServiceMock
            .Setup(service => service.GetIdempotencyKeyAsync(authenticatedUserId, "request-key", Endpoint, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingIdempotencyKey);

        this.idempotencyServiceMock
            .Setup(service => service.RemoveIdempotencyKeyIfExpiredAsync(existingIdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        this.productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(firstProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductEntity
            {
                Id = firstProductId,
                Name = "Product A",
                PriceInCents = 1500,
                IsActive = true,
            });

        this.productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(secondProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductEntity
            {
                Id = secondProductId,
                Name = "Product B",
                PriceInCents = 2300,
                IsActive = true,
            });

        this.pixPaymentServiceMock
            .Setup(service => service.CreateAsync(
                It.Is<CreatePixPaymentDto>(payment =>
                    payment.AmountInCents == 5300 &&
                    payment.ExpiresInSeconds == 3600 &&
                    payment.Metadata["userId"] == authenticatedUserId.ToString()),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreatePixPaymentResultDto
            {
                PaymentId = "pix_123",
                Status = "PENDING",
                BrCode = "000201",
                BrCodeBase64 = "data:image/png;base64,abc",
                ExpiresAt = DateTime.UtcNow.AddHours(1),
            });

        var result = await this.sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);

        Assert.Equal(5300, result.TotalPriceInCents);
        Assert.Equal("pix_123", result.PaymentId);
        Assert.Equal(OrderPaymentStatus.Pending.ToString(), result.PaymentStatus);

        this.orderRepositoryMock.Verify(
            repository => repository.CreateAsync(
            It.Is<OrderEntity>(order =>
                order.Id != Guid.Empty &&
                order.UserId == authenticatedUserId &&
                order.TotalPriceInCents == 5300 &&
                order.Status == OrderStatus.AwaitingPayment &&
                order.PaymentStatus == OrderPaymentStatus.Pending &&
                order.ProductOrders.Count == 2 &&
                order.ProductOrders.All(item =>
                    !string.IsNullOrWhiteSpace(item.ProductName) &&
                    item.TotalPriceInCents > 0)),
            It.IsAny<CancellationToken>()), Times.Once);

        this.idempotencyServiceMock.Verify(
            service => service.CreateIdempotencyKeyAsync(
            It.Is<CreateIdempotencyKeyDto>(record =>
                record.AuthenticatedUserId == authenticatedUserId &&
                record.IdempotencyKey == "request-key" &&
                record.Endpoint == Endpoint &&
                ReferenceEquals(record.RequestObject, dto) &&
                record.StatusCode == StatusCodes.Status200OK &&
                record.ResponseObject is CreateOrderResultDto),
            It.IsAny<CancellationToken>()), Times.Once);

        this.unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenIdempotencyKeyDoesNotExistAndRequestIsValid_CreatesOrderPaymentAndIdempotencyRecord()
    {
        var authenticatedUserId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var dto = new CreateOrderDto
        {
            Items =
            [
                new CreateOrderItemDto { ProductId = productId, Quantity = 2 }
            ],
        };

        this.authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);
        this.userRepositoryMock
            .Setup(repository => repository.FindByIdAsync(authenticatedUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(BuildUser(authenticatedUserId));

        this.idempotencyServiceMock
            .Setup(service => service.GetIdempotencyKeyAsync(authenticatedUserId, "request-key", Endpoint, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IdempotencyKey?)null);

        this.productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductEntity
            {
                Id = productId,
                PriceInCents = 1200,
                IsActive = true,
            });

        this.pixPaymentServiceMock
            .Setup(service => service.CreateAsync(It.IsAny<CreatePixPaymentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreatePixPaymentResultDto
            {
                PaymentId = "pix_456",
                Status = "PENDING",
                BrCode = "000202",
                BrCodeBase64 = "data:image/png;base64,def",
                ExpiresAt = DateTime.UtcNow.AddHours(1),
            });

        var result = await this.sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);

        Assert.Equal(2400, result.TotalPriceInCents);
        Assert.Equal("pix_456", result.PaymentId);
        Assert.Equal(OrderPaymentStatus.Pending.ToString(), result.PaymentStatus);
        this.pixPaymentServiceMock.Verify(
            service => service.CreateAsync(It.IsAny<CreatePixPaymentDto>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static CreateOrderDto BuildDto()
    {
        return new CreateOrderDto
        {
            Items =
            [
                new CreateOrderItemDto { ProductId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Quantity = 2 },
                new CreateOrderItemDto { ProductId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Quantity = 1 }
            ],
        };
    }

    private static AutoriaStore.Domain.Entities.User BuildUser(Guid userId)
    {
        return new AutoriaStore.Domain.Entities.User(
            "Customer",
            "customer@autoria.store",
            "hash",
            UserRole.Customer,
            DateTime.UtcNow)
        {
            Id = userId,
        };
    }
}

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.CreateOrder;
using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using OrderEntity = AutoriaStore.Domain.Entities.Order;
using ProductEntity = AutoriaStore.Domain.Entities.Product;

namespace AutoriaStore.UnitTests.UseCases.Order;

public class CreateOrderUseCaseTests
{
    private const string Endpoint = "POST:/api/order";

    private readonly Mock<IAuthenticatedUserService> _authenticatedUserServiceMock;
    private readonly Mock<IIdempotencyService> _idempotencyServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly CreateOrderUseCase _sut;

    public CreateOrderUseCaseTests()
    {
        _authenticatedUserServiceMock = new Mock<IAuthenticatedUserService>();
        _idempotencyServiceMock = new Mock<IIdempotencyService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _unitOfWorkMock.Setup(u => u.Order).Returns(_orderRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.Product).Returns(_productRepositoryMock.Object);
        _orderRepositoryMock
            .Setup(repository => repository.CreateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((OrderEntity order, CancellationToken _) => order);

        _sut = new CreateOrderUseCase(
            _authenticatedUserServiceMock.Object,
            _idempotencyServiceMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserIsNotAuthenticated_ThrowsUnauthorizeException()
    {
        var dto = BuildDto();

        _authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns((Guid?)null);

        var act = () => _sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizeException>(act);

        Assert.Equal(ExceptionMessages.USER_NOT_AUTHENTICATED, exception.Message);
        _idempotencyServiceMock.Verify(
            service => service.GetIdempotencyKeyAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenIdempotencyKeyIsMissing_ThrowsBadRequestException()
    {
        var authenticatedUserId = Guid.NewGuid();
        var dto = BuildDto();

        _authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);

        var act = () => _sut.ExecuteAsync(dto, " ", Endpoint, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<BadRequestException>(act);

        Assert.Equal(ExceptionMessages.IDEMPOTENCY_KEY_REQUIRED, exception.Message);
        _idempotencyServiceMock.Verify(
            service => service.GetIdempotencyKeyAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenItemsAreEmpty_ThrowsConflictException()
    {
        var authenticatedUserId = Guid.NewGuid();
        var dto = new CreateOrderDto { Items = [] };

        _authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);

        var act = () => _sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);

        Assert.Equal(ExceptionMessages.ORDER_ITEMS_REQUIRED, exception.Message);
        _idempotencyServiceMock.Verify(
            service => service.GetIdempotencyKeyAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenIdempotencyKeyDoesNotExist_ReturnsWithoutCreatingOrder()
    {
        var authenticatedUserId = Guid.NewGuid();
        var dto = BuildDto();

        _authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);

        _idempotencyServiceMock
            .Setup(service => service.GetIdempotencyKeyAsync(
                authenticatedUserId,
                "request-key",
                Endpoint,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((IdempotencyKey?)null);

        await _sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);

        _idempotencyServiceMock.Verify(
            service => service.RemoveIdempotencyKeyIfExpiredAsync(It.IsAny<IdempotencyKey>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _orderRepositoryMock.Verify(
            repository => repository.CreateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenIdempotencyKeyExistsButIsNotExpired_ReturnsWithoutCreatingOrder()
    {
        var authenticatedUserId = Guid.NewGuid();
        var dto = BuildDto();
        var existingIdempotencyKey = new IdempotencyKey
        {
            Id = Guid.NewGuid(),
            UserId = authenticatedUserId,
            Key = "request-key",
            Endpoint = Endpoint,
            RequestHash = "hash",
            ResponseStatus = StatusCodes.Status204NoContent,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            CreatedAt = DateTime.UtcNow
        };

        _authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);

        _idempotencyServiceMock
            .Setup(service => service.GetIdempotencyKeyAsync(authenticatedUserId, "request-key", Endpoint, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingIdempotencyKey);

        _idempotencyServiceMock
            .Setup(service => service.RemoveIdempotencyKeyIfExpiredAsync(existingIdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        await _sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);

        _orderRepositoryMock.Verify(
            repository => repository.CreateAsync(It.IsAny<OrderEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _idempotencyServiceMock.Verify(
            service => service.CreateIdempotencyKeyAsync(It.IsAny<CreateIdempotencyKeyDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenExistingIdempotencyKeyIsExpiredAndProductIsMissing_ThrowsNotFoundException()
    {
        var authenticatedUserId = Guid.NewGuid();
        var dto = BuildDto();
        var existingIdempotencyKey = new IdempotencyKey
        {
            Id = Guid.NewGuid(),
            UserId = authenticatedUserId,
            Key = "request-key",
            Endpoint = Endpoint,
            RequestHash = "hash",
            ResponseStatus = StatusCodes.Status204NoContent,
            ExpiresAt = DateTime.UtcNow.AddHours(-1),
            CreatedAt = DateTime.UtcNow
        };

        _authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);

        _idempotencyServiceMock
            .Setup(service => service.GetIdempotencyKeyAsync(authenticatedUserId, "request-key", Endpoint, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingIdempotencyKey);

        _idempotencyServiceMock
            .Setup(service => service.RemoveIdempotencyKeyIfExpiredAsync(existingIdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductEntity?)null);

        var act = () => _sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);
        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

        Assert.Equal(ExceptionMessages.PRODUCT_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenExistingIdempotencyKeyIsExpiredAndRequestIsValid_CreatesOrderAndIdempotencyRecord()
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
            ResponseStatus = StatusCodes.Status204NoContent,
            ExpiresAt = DateTime.UtcNow.AddHours(-1),
            CreatedAt = DateTime.UtcNow
        };

        var dto = new CreateOrderDto
        {
            Items =
            [
                new CreateOrderItemDto { ProductId = firstProductId, Quantity = 2 },
                new CreateOrderItemDto { ProductId = secondProductId, Quantity = 1 }
            ]
        };

        _authenticatedUserServiceMock
            .Setup(service => service.GetUserId())
            .Returns(authenticatedUserId);

        _idempotencyServiceMock
            .Setup(service => service.GetIdempotencyKeyAsync(authenticatedUserId, "request-key", Endpoint, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingIdempotencyKey);

        _idempotencyServiceMock
            .Setup(service => service.RemoveIdempotencyKeyIfExpiredAsync(existingIdempotencyKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(firstProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductEntity
            {
                Id = firstProductId,
                PriceInCents = 1500,
                IsActive = true
            });

        _productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(secondProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ProductEntity
            {
                Id = secondProductId,
                PriceInCents = 2300,
                IsActive = true
            });

        await _sut.ExecuteAsync(dto, "request-key", Endpoint, CancellationToken.None);

        _orderRepositoryMock.Verify(repository => repository.CreateAsync(
            It.Is<OrderEntity>(order =>
                order.Id != Guid.Empty &&
                order.UserId == authenticatedUserId &&
                order.TotalPriceInCents == 5300 &&
                order.ProductOrders.Count == 2),
            It.IsAny<CancellationToken>()), Times.Once);

        _idempotencyServiceMock.Verify(service => service.CreateIdempotencyKeyAsync(
            It.Is<CreateIdempotencyKeyDto>(record =>
                record.AuthenticatedUserId == authenticatedUserId &&
                record.IdempotencyKey == "request-key" &&
                record.Endpoint == Endpoint &&
                ReferenceEquals(record.RequestObject, dto) &&
                record.StatusCode == StatusCodes.Status204NoContent &&
                record.ResponseObject == null),
            It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork => unitOfWork.SaveChangesAsync(), Times.Once);
    }

    private static CreateOrderDto BuildDto()
    {
        return new CreateOrderDto
        {
            Items =
            [
                new CreateOrderItemDto { ProductId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Quantity = 2 },
                new CreateOrderItemDto { ProductId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Quantity = 1 }
            ]
        };
    }

}

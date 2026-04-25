using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.CalculateShipping;
using AutoriaStore.Domain.Dto.Clients;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Clients;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using ProductEntity = AutoriaStore.Domain.Entities.Product;

namespace AutoriaStore.UnitTests.UseCases.Shipping;

public class CalculateShippingUseCaseTests
{
    private readonly MemoryCache _memoryCache;
    private readonly Mock<IPostageHttpClient> _postageHttpClientMock;
    private readonly Mock<IEnvironmentVariablesService> _environmentVariablesServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly CalculateShippingUseCase _sut;

    public CalculateShippingUseCaseTests()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _postageHttpClientMock = new Mock<IPostageHttpClient>();
        _environmentVariablesServiceMock = new Mock<IEnvironmentVariablesService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _environmentVariablesServiceMock
            .SetupGet(service => service.OriginPostalCode)
            .Returns("12345678");

        _unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Product)
            .Returns(_productRepositoryMock.Object);

        _sut = new CalculateShippingUseCase(
            _memoryCache,
            _postageHttpClientMock.Object,
            _environmentVariablesServiceMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenDestinationPostalCodeMatchesOrigin_ReturnsFreeShipping()
    {
        var dto = new CalculateShippingDto
        {
            ProductId = Guid.NewGuid(),
            DestinationPostalCode = "12345678"
        };

        var result = await _sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Equal(0, result.ShippingPriceInCents);
        Assert.Equal(DateTime.Today.AddDays(2), result.EstimationDeliveryDate);
        _productRepositoryMock.Verify(
            repository => repository.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _postageHttpClientMock.Verify(
            client => client.GetDeliveryTimeAsync(It.IsAny<GetDeliveryTimeDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _postageHttpClientMock.Verify(
            client => client.GetShippingPriceAsync(It.IsAny<GetShippingPriceDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductIsNotFound_ThrowsNotFoundException()
    {
        var dto = new CalculateShippingDto
        {
            ProductId = Guid.NewGuid(),
            DestinationPostalCode = "87654321"
        };

        _productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(dto.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductEntity?)null);

        var act = () => _sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

        Assert.Equal(ExceptionMessages.PRODUCT_NOT_FOUND, exception.Message);
        _postageHttpClientMock.Verify(
            client => client.GetDeliveryTimeAsync(It.IsAny<GetDeliveryTimeDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _postageHttpClientMock.Verify(
            client => client.GetShippingPriceAsync(It.IsAny<GetShippingPriceDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenShippingResultExistsInCache_ReturnsCachedResult()
    {
        var dto = new CalculateShippingDto
        {
            ProductId = Guid.NewGuid(),
            DestinationPostalCode = "87654321"
        };
        var product = BuildProduct(dto.ProductId);
        var cachedResult = new CalculateShippingResultDto
        {
            ShippingPriceInCents = 1590,
            EstimationDeliveryDate = new DateTime(2026, 4, 30)
        };

        _productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(dto.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _memoryCache.Set($"shipping:{dto.DestinationPostalCode}", cachedResult, TimeSpan.FromDays(7));

        var result = await _sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Same(cachedResult, result);
        _postageHttpClientMock.Verify(
            client => client.GetDeliveryTimeAsync(It.IsAny<GetDeliveryTimeDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _postageHttpClientMock.Verify(
            client => client.GetShippingPriceAsync(It.IsAny<GetShippingPriceDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenShippingResultIsNotCached_CalculatesAndStoresResult()
    {
        var dto = new CalculateShippingDto
        {
            ProductId = Guid.NewGuid(),
            DestinationPostalCode = "87654321"
        };
        var product = BuildProduct(dto.ProductId);
        var deliveryEstimate = new DateTime(2026, 5, 1);

        _productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(dto.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _postageHttpClientMock
            .Setup(client => client.GetDeliveryTimeAsync(
                It.Is<GetDeliveryTimeDto>(request =>
                    request.DestinationPostalCode == dto.DestinationPostalCode),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetDeliveryTimeResponseDto
            {
                EstimationDeliveryDate = deliveryEstimate
            });

        _postageHttpClientMock
            .Setup(client => client.GetShippingPriceAsync(
                It.Is<GetShippingPriceDto>(request =>
                    request.DestinationPostalCode == dto.DestinationPostalCode &&
                    request.WeightInGrams == product.WeightInGrams &&
                    request.DepthInCentimeters == product.DepthInCentimeters &&
                    request.WidthInCentimeters == product.WidthInCentimeters &&
                    request.HeightInCentimeters == product.HeightInCentimeters),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetShippingPriceResponseDto
            {
                PriceInCents = 2300
            });

        var result = await _sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Equal(2000, result.ShippingPriceInCents);
        Assert.Equal(deliveryEstimate.AddDays(2), result.EstimationDeliveryDate);

        Assert.True(
            _memoryCache.TryGetValue<CalculateShippingResultDto>($"shipping:{dto.DestinationPostalCode}",
                out var cachedResult));
        Assert.NotNull(cachedResult);
        Assert.Equal(result.ShippingPriceInCents, cachedResult!.ShippingPriceInCents);
        Assert.Equal(result.EstimationDeliveryDate, cachedResult.EstimationDeliveryDate);
    }

    private static ProductEntity BuildProduct(Guid productId)
    {
        return new ProductEntity
        {
            Id = productId,
            Name = "Notebook",
            PrintDescription = "Print",
            Description = "Description",
            WeightInGrams = 1200,
            DepthInCentimeters = 10,
            WidthInCentimeters = 20,
            HeightInCentimeters = 30,
            CreatedAt = DateTime.UtcNow
        };
    }
}

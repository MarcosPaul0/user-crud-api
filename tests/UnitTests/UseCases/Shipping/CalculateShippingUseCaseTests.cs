// <copyright file="CalculateShippingUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
    private readonly MemoryCache memoryCache;
    private readonly Mock<IPostageHttpClient> postageHttpClientMock;
    private readonly Mock<IEnvironmentVariablesService> environmentVariablesServiceMock;
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IProductRepository> productRepositoryMock;
    private readonly CalculateShippingUseCase sut;

    public CalculateShippingUseCaseTests()
    {
        this.memoryCache = new MemoryCache(new MemoryCacheOptions());
        this.postageHttpClientMock = new Mock<IPostageHttpClient>();
        this.environmentVariablesServiceMock = new Mock<IEnvironmentVariablesService>();
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.productRepositoryMock = new Mock<IProductRepository>();

        this.environmentVariablesServiceMock
            .SetupGet(service => service.OriginPostalCode)
            .Returns("12345678");

        this.unitOfWorkMock
            .Setup(unitOfWork => unitOfWork.Product)
            .Returns(this.productRepositoryMock.Object);

        this.sut = new CalculateShippingUseCase(
            this.memoryCache,
            this.postageHttpClientMock.Object,
            this.environmentVariablesServiceMock.Object,
            this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenDestinationPostalCodeMatchesOrigin_ReturnsFreeShipping()
    {
        var dto = new CalculateShippingDto
        {
            ProductId = Guid.NewGuid(),
            DestinationPostalCode = "12345678",
        };

        var result = await this.sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Equal(0, result.ShippingPriceInCents);
        Assert.Equal(DateTime.Today.AddDays(2), result.EstimationDeliveryDate);
        this.productRepositoryMock.Verify(
            repository => repository.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
        this.postageHttpClientMock.Verify(
            client => client.GetDeliveryTimeAsync(It.IsAny<GetDeliveryTimeDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
        this.postageHttpClientMock.Verify(
            client => client.GetShippingPriceAsync(It.IsAny<GetShippingPriceDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenProductIsNotFound_ThrowsNotFoundException()
    {
        var dto = new CalculateShippingDto
        {
            ProductId = Guid.NewGuid(),
            DestinationPostalCode = "87654321",
        };

        this.productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(dto.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductEntity?)null);

        var act = () => this.sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);

        Assert.Equal(ExceptionMessages.PRODUCTNOTFOUND, exception.Message);
        this.postageHttpClientMock.Verify(
            client => client.GetDeliveryTimeAsync(It.IsAny<GetDeliveryTimeDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
        this.postageHttpClientMock.Verify(
            client => client.GetShippingPriceAsync(It.IsAny<GetShippingPriceDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenShippingResultExistsInCache_ReturnsCachedResult()
    {
        var dto = new CalculateShippingDto
        {
            ProductId = Guid.NewGuid(),
            DestinationPostalCode = "87654321",
        };
        var product = BuildProduct(dto.ProductId);
        var cachedResult = new CalculateShippingResultDto
        {
            ShippingPriceInCents = 1590,
            EstimationDeliveryDate = new DateTime(2026, 4, 30, 0, 0, 0, DateTimeKind.Utc),
        };

        this.productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(dto.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        this.memoryCache.Set($"shipping:{dto.DestinationPostalCode}", cachedResult, TimeSpan.FromDays(7));

        var result = await this.sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Same(cachedResult, result);
        this.postageHttpClientMock.Verify(
            client => client.GetDeliveryTimeAsync(It.IsAny<GetDeliveryTimeDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
        this.postageHttpClientMock.Verify(
            client => client.GetShippingPriceAsync(It.IsAny<GetShippingPriceDto>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenShippingResultIsNotCached_CalculatesAndStoresResult()
    {
        var dto = new CalculateShippingDto
        {
            ProductId = Guid.NewGuid(),
            DestinationPostalCode = "87654321",
        };
        var product = BuildProduct(dto.ProductId);
        var deliveryEstimate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc);

        this.productRepositoryMock
            .Setup(repository => repository.FindByIdAsync(dto.ProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        this.postageHttpClientMock
            .Setup(client => client.GetDeliveryTimeAsync(
                It.Is<GetDeliveryTimeDto>(request =>
                    request.DestinationPostalCode == dto.DestinationPostalCode),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetDeliveryTimeResponseDto
            {
                EstimationDeliveryDate = deliveryEstimate,
            });

        this.postageHttpClientMock
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
                PriceInCents = 2300,
            });

        var result = await this.sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Equal(2000, result.ShippingPriceInCents);
        Assert.Equal(deliveryEstimate.AddDays(2), result.EstimationDeliveryDate);

        Assert.True(
            this.memoryCache.TryGetValue<CalculateShippingResultDto>(
                $"shipping:{dto.DestinationPostalCode}",
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
            CreatedAt = DateTime.UtcNow,
        };
    }
}

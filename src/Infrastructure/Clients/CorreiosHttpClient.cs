// <copyright file="CorreiosHttpClient.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Globalization;
using System.Net;
using System.Text.Json;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Dto.Clients;
using AutoriaStore.Domain.Interfaces.Clients;
using AutoriaStore.Domain.Interfaces.Services;
using AutoriaStore.Infrastructure.Clients.Responses;
using Microsoft.AspNetCore.WebUtilities;

namespace AutoriaStore.Infrastructure.Clients;

public sealed class CorreiosHttpClient : IPostageHttpClient
{
    private readonly HttpClient httpClient;

    private const string BaseUrl = "https://api.correios.com.br";

    private readonly string originPostalCode;
    private readonly string serviceCode;

    public CorreiosHttpClient(
        HttpClient httpClient,
        IEnvironmentVariablesService environmentVariablesService)
    {
        this.httpClient = httpClient;
        this.httpClient.BaseAddress = new Uri(BaseUrl);
        this.httpClient.Timeout = TimeSpan.FromSeconds(30);
        this.httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        this.httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {environmentVariablesService.PostageApiKey}");

        this.originPostalCode = environmentVariablesService.OriginPostalCode;
        this.serviceCode = environmentVariablesService.PostageApiServiceCode;
    }

    public async Task<GetShippingPriceResponseDto> GetShippingPriceAsync(
        GetShippingPriceDto getShippingPriceDto,
        CancellationToken cancellationToken = default)
    {
        var endpoint = $"/preco/v1/nacional/{this.serviceCode}";

        var queryParams = new Dictionary<string, string?>
        {
            ["cepOrigem"] = this.originPostalCode,
            ["cepDestino"] = getShippingPriceDto.DestinationPostalCode,
            ["psObjeto"] = getShippingPriceDto.WeightInGrams.ToString(),
            ["tpObjeto"] = "2",
            ["comprimento"] = getShippingPriceDto.DepthInCentimeters.ToString(),
            ["altura"] = getShippingPriceDto.HeightInCentimeters.ToString(),
            ["largura"] = getShippingPriceDto.WidthInCentimeters.ToString(),
        };

        var urlWithQuery = QueryHelpers.AddQueryString(endpoint, queryParams);

        var response = await this.httpClient.GetAsync(urlWithQuery, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException("Price not found for this postal code.");
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Postal service error");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseData = JsonSerializer.Deserialize<CorreiosPriceResponse>(content);

        if (responseData is null)
        {
            throw new Exception("Postal response parse error");
        }

        return new GetShippingPriceResponseDto()
        {
            PriceInCents = RealToCents(responseData.PcFinal),
        };
    }

    public async Task<GetDeliveryTimeResponseDto> GetDeliveryTimeAsync(
        GetDeliveryTimeDto getDeliveryTimeDto,
        CancellationToken cancellationToken = default)
    {
        var endpoint = $"/prazo/v1/nacional/{this.serviceCode}";

        var queryParams = new Dictionary<string, string?>
        {
            ["cepOrigem"] = this.originPostalCode,
            ["cepDestino"] = getDeliveryTimeDto.DestinationPostalCode,
        };

        var urlWithQuery = QueryHelpers.AddQueryString(endpoint, queryParams);

        var response = await this.httpClient.GetAsync(urlWithQuery, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException("Delivery time not found for this postal code.");
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Postal service error");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseData = JsonSerializer.Deserialize<CorreiosTimeResponse>(content);

        if (responseData is null)
        {
            throw new Exception("Postal response parse error");
        }

        return new GetDeliveryTimeResponseDto()
        {
            EstimationDeliveryDate = StringToDateTime(responseData.DataMaxima),
        };
    }

    private static int RealToCents(string value)
    {
        return int.TryParse(value.Replace(",", string.Empty), out var result) ? result : 0;
    }

    private static DateTime StringToDateTime(string value)
    {
        return DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
    }
}
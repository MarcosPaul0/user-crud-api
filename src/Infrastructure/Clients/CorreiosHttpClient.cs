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
    private readonly HttpClient _httpClient;
    
    private const string BaseUrl = "https://api.correios.com.br";
    
    private readonly string _originPostalCode; 
    private readonly string _serviceCode; 

    public CorreiosHttpClient(
        HttpClient httpClient,
        IEnvironmentVariablesService environmentVariablesService)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {environmentVariablesService.PostageApiKey}");
            
        _originPostalCode = environmentVariablesService.OriginPostalCode;
        _serviceCode = environmentVariablesService.PostageApiServiceCode;
    }

    public async Task<GetShippingPriceResponseDto> GetShippingPriceAsync(
        GetShippingPriceDto getShippingPriceDto, 
        CancellationToken cancellationToken = default)
    {
        var endpoint = $"/preco/v1/nacional/{_serviceCode}";
        
        var queryParams = new Dictionary<string, string?>
        {
            ["cepOrigem"] = _originPostalCode,
            ["cepDestino"] = getShippingPriceDto.DestinationPostalCode,
            ["psObjeto"] = getShippingPriceDto.WeightInGrams.ToString(),
            ["tpObjeto"] = "2",
            ["comprimento"] = getShippingPriceDto.DepthInCentimeters.ToString(),
            ["altura"] = getShippingPriceDto.HeightInCentimeters.ToString(),
            ["largura"] = getShippingPriceDto.WidthInCentimeters.ToString(),
        };
        
        var urlWithQuery = QueryHelpers.AddQueryString(endpoint, queryParams);
        
        var response = await _httpClient.GetAsync(urlWithQuery, cancellationToken);

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
        var endpoint = $"/prazo/v1/nacional/{_serviceCode}";
        
        var queryParams = new Dictionary<string, string?>
        {
            ["cepOrigem"] = _originPostalCode,
            ["cepDestino"] = getDeliveryTimeDto.DestinationPostalCode,
        };
        
        var urlWithQuery = QueryHelpers.AddQueryString(endpoint, queryParams);
        
        var response = await _httpClient.GetAsync(urlWithQuery, cancellationToken);
        
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
        return int.TryParse(value.Replace(",", ""), out var result) ? result : 0;
    }

    private static DateTime StringToDateTime(string value)
    {
        return DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
    }
}
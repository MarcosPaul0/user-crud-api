// <copyright file="AbacatePayPixPaymentService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace AutoriaStore.Infrastructure.Services;

public sealed class AbacatePayPixPaymentService(
    HttpClient httpClient,
    IEnvironmentVariablesService environmentVariablesService) : IPixPaymentService
{
    public async Task<CreatePixPaymentResultDto> CreateAsync(
        CreatePixPaymentDto createPixPaymentDto,
        CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "v2/transparents/create");
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", environmentVariablesService.AbacatePayApiKey);
        request.Content = new StringContent(
            JsonSerializer.Serialize(new AbacatePayCreatePixRequest(
                "PIX",
                new AbacatePayCreatePixRequestData(
                    createPixPaymentDto.AmountInCents,
                    createPixPaymentDto.ExpiresInSeconds,
                    createPixPaymentDto.Description,
                    createPixPaymentDto.ExternalId,
                    createPixPaymentDto.Metadata,
                    createPixPaymentDto.Customer is null
                        ? null
                        : new AbacatePayCreatePixRequestCustomer(
                            createPixPaymentDto.Customer.Name,
                            createPixPaymentDto.Customer.Email,
                            createPixPaymentDto.Customer.TaxId,
                            createPixPaymentDto.Customer.Cellphone)))),
            Encoding.UTF8,
            "application/json");

        var response = await httpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Failed to create PIX payment using Abacate Pay. StatusCode: {(int)response.StatusCode}. Response: {responseContent}");
        }

        var paymentResponse = JsonSerializer.Deserialize<AbacatePayCreatePixResponse>(
            responseContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

        if (paymentResponse?.data is null)
        {
            throw new InvalidOperationException("Abacate Pay returned an invalid PIX payment response.");
        }

        return new CreatePixPaymentResultDto
        {
            PaymentId = paymentResponse.data.id,
            Status = paymentResponse.data.status,
            BrCode = paymentResponse.data.brCode,
            BrCodeBase64 = paymentResponse.data.brCodeBase64,
            ExpiresAt = paymentResponse.data.expiresAt,
        };
    }

    public async Task<GetPixPaymentStatusResultDto> GetStatusAsync(
        string paymentId,
        CancellationToken cancellationToken)
    {
        var url = QueryHelpers.AddQueryString("v2/transparents/check", "id", paymentId);
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", environmentVariablesService.AbacatePayApiKey);

        var response = await httpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Failed to fetch PIX payment status using Abacate Pay. StatusCode: {(int)response.StatusCode}. Response: {responseContent}");
        }

        var paymentResponse = JsonSerializer.Deserialize<AbacatePayGetPixStatusResponse>(
            responseContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

        if (paymentResponse?.data is null)
        {
            throw new InvalidOperationException("Abacate Pay returned an invalid PIX payment status response.");
        }

        return new GetPixPaymentStatusResultDto
        {
            PaymentId = paymentResponse.data.id,
            Status = paymentResponse.data.status,
            ExpiresAt = paymentResponse.data.expiresAt,
            ReceiptUrl = paymentResponse.data.receiptUrl,
        };
    }

    private sealed record AbacatePayCreatePixRequest(
        string method,
        AbacatePayCreatePixRequestData data);

    private sealed record AbacatePayCreatePixRequestData(
        int amount,
        int expiresIn,
        string description,
        string externalId,
        IReadOnlyDictionary<string, string> metadata,
        AbacatePayCreatePixRequestCustomer? customer);

    private sealed record AbacatePayCreatePixRequestCustomer(
        string? name,
        string? email,
        string? taxId,
        string? cellphone);

    private sealed record AbacatePayCreatePixResponse(AbacatePayCreatePixResponseData? data);

    private sealed record AbacatePayCreatePixResponseData(
        string id,
        string status,
        string brCode,
        string brCodeBase64,
        DateTime expiresAt);

    private sealed record AbacatePayGetPixStatusResponse(AbacatePayGetPixStatusResponseData? data);

    private sealed record AbacatePayGetPixStatusResponseData(
        string id,
        string status,
        DateTime? expiresAt,
        string? receiptUrl);
}

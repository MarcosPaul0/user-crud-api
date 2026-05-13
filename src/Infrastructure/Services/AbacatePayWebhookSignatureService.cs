// <copyright file="AbacatePayWebhookSignatureService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Security.Cryptography;
using System.Text;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.Infrastructure.Services;

public sealed class AbacatePayWebhookSignatureService(
    IEnvironmentVariablesService environmentVariablesService) : IAbacatePayWebhookSignatureService
{
    public bool IsValid(string rawBody, string signature)
    {
        if (string.IsNullOrWhiteSpace(rawBody) || string.IsNullOrWhiteSpace(signature))
        {
            return false;
        }

        var rawBodyBytes = Encoding.UTF8.GetBytes(rawBody);
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(environmentVariablesService.AbacatePayWebhookPublicKey));
        var computedSignature = Convert.ToBase64String(hmac.ComputeHash(rawBodyBytes));

        var computedBytes = Encoding.UTF8.GetBytes(computedSignature);
        var providedBytes = Encoding.UTF8.GetBytes(signature);

        return computedBytes.Length == providedBytes.Length &&
               CryptographicOperations.FixedTimeEquals(computedBytes, providedBytes);
    }
}

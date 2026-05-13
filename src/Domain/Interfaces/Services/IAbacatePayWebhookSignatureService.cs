// <copyright file="IAbacatePayWebhookSignatureService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Interfaces.Services;

public interface IAbacatePayWebhookSignatureService
{
    bool IsValid(string rawBody, string signature);
}

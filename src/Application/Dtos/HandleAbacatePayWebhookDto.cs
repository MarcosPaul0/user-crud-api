// <copyright file="HandleAbacatePayWebhookDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Dtos;

public sealed record HandleAbacatePayWebhookDto
{
    required public string WebhookSecret { get; init; }
    required public string Signature { get; init; }
    required public string RawBody { get; init; }
}

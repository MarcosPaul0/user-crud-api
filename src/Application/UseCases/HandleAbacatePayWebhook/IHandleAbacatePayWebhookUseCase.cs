// <copyright file="IHandleAbacatePayWebhookUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.HandleAbacatePayWebhook;

public interface IHandleAbacatePayWebhookUseCase
{
    Task ExecuteAsync(HandleAbacatePayWebhookDto handleAbacatePayWebhookDto, CancellationToken cancellationToken);
}

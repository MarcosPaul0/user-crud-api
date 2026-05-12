// <copyright file="AbacatePayWebhookController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text;
using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.HandleAbacatePayWebhook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/webhooks/abacate-pay")]
public sealed class AbacatePayWebhookController(
    IHandleAbacatePayWebhookUseCase handleAbacatePayWebhookUseCase) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> HandleAsync(
        [FromQuery] string webhookSecret,
        [FromHeader(Name = "X-Webhook-Signature")] string signature,
        CancellationToken cancellationToken)
    {
        this.Request.EnableBuffering();

        using var reader = new StreamReader(this.Request.Body, Encoding.UTF8, leaveOpen: true);
        var rawBody = await reader.ReadToEndAsync(cancellationToken);
        this.Request.Body.Position = 0;

        await handleAbacatePayWebhookUseCase.ExecuteAsync(
            new HandleAbacatePayWebhookDto
            {
                WebhookSecret = webhookSecret,
                Signature = signature,
                RawBody = rawBody,
            }, cancellationToken);

        return this.Ok();
    }
}

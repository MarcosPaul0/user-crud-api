// <copyright file="ResendEmailService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.Infrastructure.Services;

public sealed class ResendEmailService(
    HttpClient httpClient,
    IEnvironmentVariablesService environmentVariablesService) : IEmailService
{
    public async Task SendAsync(SendEmailDto sendEmailDto, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "emails");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", environmentVariablesService.ResendApiKey);
        request.Content = new StringContent(
            JsonSerializer.Serialize(new ResendSendEmailRequest(
                environmentVariablesService.ResendFromEmail,
                [sendEmailDto.To],
                sendEmailDto.Subject,
                sendEmailDto.HtmlBody)),
            Encoding.UTF8,
            "application/json");

        var response = await httpClient.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        throw new InvalidOperationException(
            $"Failed to send email using Resend. StatusCode: {(int)response.StatusCode}. Response: {responseContent}");
    }

    private sealed record ResendSendEmailRequest(
        string from,
        IReadOnlyCollection<string> to,
        string subject,
        string html);
}

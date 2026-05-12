// <copyright file="SendEmailDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Services;

public sealed class SendEmailDto
{
    required public string To { get; init; }

    required public string Subject { get; init; }

    required public string HtmlBody { get; init; }
}

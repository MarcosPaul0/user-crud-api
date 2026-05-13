// <copyright file="IEmailService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Dto.Services;

namespace AutoriaStore.Domain.Interfaces.Services;

public interface IEmailService
{
    Task SendAsync(SendEmailDto sendEmailDto, CancellationToken cancellationToken);
}

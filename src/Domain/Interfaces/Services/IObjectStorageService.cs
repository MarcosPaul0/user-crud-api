// <copyright file="IObjectStorageService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;

namespace AutoriaStore.Domain.Interfaces.Services;

public interface IObjectStorageService
{
    Task<string> UploadAsync(
        IFormFile file,
        string objectKey,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        string objectKey,
        CancellationToken cancellationToken);
}
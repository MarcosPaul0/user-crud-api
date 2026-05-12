// <copyright file="AllowedImageExtensionsAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AutoriaStore.Application.Attributes;

public class AllowedImageExtensionsAttribute(params string[] extensions) : ValidationAttribute
{
    private readonly ValidationResult errorMessage = new ($"Only files with the {string.Join(", ", extensions)} extension are allowed.");

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case IFormFile file:
                if (!this.IsValidExtension(file))
                {
                    return this.errorMessage;
                }

                break;

            case List<IFormFile> files:
                if (files.Any(currentFile => !this.IsValidExtension(currentFile)))
                {
                    return this.errorMessage;
                }

                break;
        }

        return ValidationResult.Success;
    }

    private bool IsValidExtension(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        return extensions.Contains(extension);
    }
}
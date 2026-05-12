// <copyright file="MaxFileSizeAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AutoriaStore.Application.Attributes;

public class MaxFileSizeAttribute(int maxFileSizeInMb) : ValidationAttribute
{
    private readonly ValidationResult errorMessage = new ($"The maximum allowed size per file is{maxFileSizeInMb}MB");

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case IFormFile file:
                if (!this.IsValidFileSize(file))
                {
                    return this.errorMessage;
                }

                break;

            case List<IFormFile> files:
                if (files.Any(currentFile => !this.IsValidFileSize(currentFile)))
                {
                    return this.errorMessage;
                }

                break;
        }

        return ValidationResult.Success;
    }

    private bool IsValidFileSize(IFormFile file)
    {
        return file.Length <= maxFileSizeInMb * 1024 * 1024;
    }
}
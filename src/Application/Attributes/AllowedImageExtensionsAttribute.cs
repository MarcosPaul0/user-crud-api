using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace UserCrud.Application.Attributes;

public class AllowedImageExtensionsAttribute(params string[] extensions) : ValidationAttribute
{
    private readonly ValidationResult _errorMessage = new($"Only files with the {string.Join(", ", extensions)} extension are allowed.");
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case IFormFile file:
                if (!IsValidExtension(file))
                {
                    return _errorMessage;
                }
                break;
            
            case List<IFormFile> files:
                if (files.Any(currentFile => !IsValidExtension(currentFile)))
                {
                    return _errorMessage;
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
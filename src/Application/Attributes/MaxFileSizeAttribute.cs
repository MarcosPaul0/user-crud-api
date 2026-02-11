using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace UserCrud.Application.Attributes;

public class MaxFileSizeAttribute(int maxFileSizeInMb) : ValidationAttribute
{
    private readonly ValidationResult _errorMessage = new($"The maximum allowed size per file is{maxFileSizeInMb}MB");
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case IFormFile file:
                if (!IsValidFileSize(file))
                {
                    return _errorMessage;
                }
                break;
            
            case List<IFormFile> files:
                if (files.Any(currentFile => !IsValidFileSize(currentFile)))
                {
                    return _errorMessage;
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
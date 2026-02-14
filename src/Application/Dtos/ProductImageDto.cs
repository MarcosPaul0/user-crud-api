using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using UserCrud.Application.Attributes;

namespace UserCrud.Application.Dtos;

public class ProductImageDto
{
    public Guid? Id { get; init; }
    
    [AllowedImageExtensions(".jpg", ".jpeg", ".png", ".webp")]
    public IFormFile? File { get; init; }
    
    [Required]
    [Range(1, 5)]
    public byte DisplayOrder { get; init; }
}
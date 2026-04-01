using System.ComponentModel.DataAnnotations;
using AutoriaStore.Application.Attributes;
using Microsoft.AspNetCore.Http;

namespace AutoriaStore.Application.Dtos;

public class ProductImageDto
{
    public Guid? Id { get; init; }
    
    [AllowedImageExtensions(".jpg", ".jpeg", ".png", ".webp")]
    public IFormFile? File { get; init; }
    
    [Required]
    [Range(1, 4)]
    public byte DisplayOrder { get; init; }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using UserCrud.Application.Attributes;

namespace UserCrud.Application.Dtos;

public record SetProductImagesDto
{
    [Required]
    public List<ProductImageDto> Images { get; init; }
}
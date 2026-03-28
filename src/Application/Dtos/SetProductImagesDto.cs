using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public record SetProductImagesDto
{
    [Required]
    public List<ProductImageDto> Images { get; init; }
}
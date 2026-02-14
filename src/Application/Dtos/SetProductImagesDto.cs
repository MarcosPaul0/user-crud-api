using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public record SetProductImagesDto
{
    [Required]
    [MinLength(5)]
    [MaxLength(5)]
    public List<ProductImageDto> Images { get; init; }
}
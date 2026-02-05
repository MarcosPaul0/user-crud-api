using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public record UpdateProductCategoryDto
{
    [Required]
    [StringLength(50, MinimumLength = 10)]
    public string Category { get; init; }
}
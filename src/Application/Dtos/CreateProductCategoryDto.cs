using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public class CreateProductCategoryDto
{
    [Required]
    [StringLength(50, MinimumLength = 10)]
    public string Category { get; init; }
}
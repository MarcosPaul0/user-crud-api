using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public record UpdateProductCategoryDto
{
    [StringLength(50, MinimumLength = 10)]
    public string? Category { get; init; }
    
    public bool? IsActive { get; init; }
}
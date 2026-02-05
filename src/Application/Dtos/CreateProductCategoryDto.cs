using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public class CreateProductCategoryDto
{
    [Required]
    [StringLength(50, MinimumLength = 10)]
    public string Category { get; init; }
}
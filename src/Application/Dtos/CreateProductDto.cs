using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public class CreateProductDto
{
    [Required]
    [StringLength(100, MinimumLength = 10)]
    public string Name { get; init; }
    
    [Required]
    [StringLength(1200, MinimumLength = 10)]
    public string Description { get; init; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int PriceInCents { get; init; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int ProductionTimeInDays { get; init; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int StockQuantity { get; init; }
    
    [Required]
    public Guid ProductCategoryId { get; init; }
}
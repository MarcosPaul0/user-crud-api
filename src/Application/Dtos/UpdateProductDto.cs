using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public record UpdateProductDto
{
    [StringLength(100, MinimumLength = 10)]
    public string? Name { get; init; }
    
    [StringLength(1200, MinimumLength = 10)]
    public string? Description { get; init; }
    
    [Range(1, int.MaxValue)]
    public int? PriceInCents { get; init; }
    
    [Range(1, int.MaxValue)]
    public int? ProductionTimeInMinutes { get; init; }
    
    [Range(1, int.MaxValue)]
    public int? StockQuantity { get; init; }
    
    [Range(0, 30)]
    public byte? DiscountPercentage { get; init; }
    
    public Guid? ProductCategoryId { get; init; }
    
    public bool? IsActive { get; init; }
}
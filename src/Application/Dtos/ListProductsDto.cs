using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public record ListProductsDto : PaginationDto
{
    [StringLength(100, MinimumLength = 3)]
    public string? Name { get; init; }
    
    public Guid? ProductCategoryId { get; init; }
}
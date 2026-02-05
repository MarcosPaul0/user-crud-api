using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public record ListProductDto : PaginationDto
{
    [StringLength(100, MinimumLength = 10)]
    public string? Name { get; init; }
    
    public Guid? ProductCategoryId { get; init; }
}
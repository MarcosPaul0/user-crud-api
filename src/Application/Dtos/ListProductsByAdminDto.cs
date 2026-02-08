using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public record ListProductsByAdminDto : PaginationDto
{
    [StringLength(100, MinimumLength = 3)]
    public string? Name { get; init; }
    
    public Guid? ProductCategoryId { get; init; }
    
    public bool? IsActive { get; init; }
}
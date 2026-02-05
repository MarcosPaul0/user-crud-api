using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public record PaginationDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Page { get; init; }
    
    [Required]
    [Range(1, 100)]
    public int ItemsPerPage { get; init; }
}
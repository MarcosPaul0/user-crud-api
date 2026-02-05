using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public record LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }
    
    [Required]
    [StringLength(225, MinimumLength = 10)]
    public string Password { get; init; }
}
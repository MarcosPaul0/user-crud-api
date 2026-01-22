using System.ComponentModel.DataAnnotations;

namespace UserCrud.Application.Dtos;

public record CreateUserDto
{
    [Required]
    [StringLength(50, MinimumLength = 10)]
    public string Name { get; init; }
    
    [Required]
    [EmailAddress]
    [StringLength(255, MinimumLength = 1)]
    public string Email { get; init; }
    
    [Required]
    [MinLength(10)]
    [StringLength(50, MinimumLength = 10)]
    public string Password { get; init; }
}
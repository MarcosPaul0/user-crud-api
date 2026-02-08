using System.ComponentModel.DataAnnotations;
using UserCrud.Domain.Enums;

namespace UserCrud.Application.Dtos;

public record ListUsersDto : PaginationDto
{
    [StringLength(50, MinimumLength = 3)]
    public string? Name { get; init; }
    
    [EnumDataType(typeof(UserRole))]
    public UserRole? Role { get; init; }
}
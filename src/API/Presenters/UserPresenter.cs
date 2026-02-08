using UserCrud.API.Dtos;
using UserCrud.API.Helpers;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Presenters;

public static class UserPresenter
{
    public static UserResponseDto ToHttp(User user)
    {
        return new UserResponseDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
    
    public static PaginationResponseDto<UserResponseDto> ToHttp(IEnumerable<User> users, int count, int page, int itemsPerPage)
    {
        var usersResponse = users.Select(ToHttp);
        
        return PaginationHelper.FormatResponse(usersResponse, count, page, itemsPerPage);
    }
}
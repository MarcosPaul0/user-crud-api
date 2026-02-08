using UserCrud.Application.Dtos;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.ListUsers;

public class ListUserUseCase(IUserRepository userRepository) : IListUserUseCase
{
    public async Task<(IEnumerable<User>, int)> ExecuteAsync(
        ListUsersDto listUsersDto, 
        CancellationToken cancellationToken)
    {
        var usersFilter = new User(
            listUsersDto.Name,
            listUsersDto.Role);
        
        var users = await userRepository.FindAllAsync(
            usersFilter,
            listUsersDto.Page,
            listUsersDto.ItemsPerPage,
            cancellationToken);
        
        var usersCount = await userRepository.CountAsync(usersFilter, cancellationToken);

        return (users, usersCount);
    }
}
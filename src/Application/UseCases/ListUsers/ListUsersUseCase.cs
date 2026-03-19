using UserCrud.Application.Dtos;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.ListUsers;

public sealed class ListUserUseCase(IUnitOfWork unitOfWork) : IListUserUseCase
{
    public async Task<(IEnumerable<User>, int)> ExecuteAsync(
        ListUsersDto listUsersDto, 
        CancellationToken cancellationToken)
    {
        var usersFilter = new User(
            listUsersDto.Name,
            listUsersDto.Role);
        
        var users = await unitOfWork.User.FindAllAsync(
            usersFilter,
            listUsersDto.Page,
            listUsersDto.ItemsPerPage,
            cancellationToken);
        
        var usersCount = await unitOfWork.User.CountAsync(usersFilter, cancellationToken);

        return (users, usersCount);
    }
}
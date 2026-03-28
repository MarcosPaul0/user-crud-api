using AutoriaStore.Application.Dtos;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;

namespace AutoriaStore.Application.UseCases.ListUsers;

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
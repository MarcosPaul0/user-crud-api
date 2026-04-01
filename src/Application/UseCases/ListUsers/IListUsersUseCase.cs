using AutoriaStore.Application.Dtos;
using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.ListUsers;

public interface IListUserUseCase
{
    Task<(IEnumerable<User>, int)> ExecuteAsync(ListUsersDto listUsersDto, CancellationToken cancellationToken);
}
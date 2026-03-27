using AutoriaStore.Domain.Entities;
using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.ListUsers;

public interface IListUserUseCase
{
    Task<(IEnumerable<User>, int)> ExecuteAsync(ListUsersDto listUsersDto, CancellationToken cancellationToken);
}
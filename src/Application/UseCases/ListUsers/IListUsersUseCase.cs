using UserCrud.Application.Dtos;
using UserCrud.Domain.Entities;

namespace UserCrud.Application.UseCases.ListUsers;

public interface IListUserUseCase
{
    Task<(IEnumerable<User>, int)> ExecuteAsync(ListUsersDto listUsersDto, CancellationToken cancellationToken);
}
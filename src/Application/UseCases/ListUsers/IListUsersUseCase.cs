using UserCrud.Domain.Entities;

namespace UserCrud.Application.UseCases.ListUsers;

public interface IListUserUseCase
{
    Task<IEnumerable<User>> ExecuteAsync(CancellationToken cancellationToken);
}
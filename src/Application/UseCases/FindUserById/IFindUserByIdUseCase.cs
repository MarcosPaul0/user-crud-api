using AutoriaStore.Domain.Entities;

namespace UserCrud.Application.UseCases.FindUserById;

public interface IFindUserByIdUseCase
{
    Task<User> ExecuteAsync(Guid userId, CancellationToken cancellationToken);
}
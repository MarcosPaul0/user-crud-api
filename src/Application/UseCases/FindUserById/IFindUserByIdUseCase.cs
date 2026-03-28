using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.FindUserById;

public interface IFindUserByIdUseCase
{
    Task<User> ExecuteAsync(Guid userId, CancellationToken cancellationToken);
}
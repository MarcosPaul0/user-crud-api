using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.UpdateUser;

public interface IUpdateUserUseCase
{
    Task ExecuteAsync(Guid userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken);
}
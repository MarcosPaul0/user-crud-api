using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.UpdateUser;

public interface IUpdateUserUseCase
{
    Task ExecuteAsync(Guid userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken);
}
using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.CreateUser;

public interface ICreateUserUseCase
{
    Task ExecuteAsync(CreateUserDto createUserDto, CancellationToken cancellationToken);
}
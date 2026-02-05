using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.CreateUser;

public interface ICreateCustomerUseCase
{
    Task ExecuteAsync(CreateUserDto createUserDto, CancellationToken cancellationToken);
}
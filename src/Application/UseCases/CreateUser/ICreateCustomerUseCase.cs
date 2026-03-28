using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.CreateUser;

public interface ICreateCustomerUseCase
{
    Task ExecuteAsync(CreateUserDto createUserDto, CancellationToken cancellationToken);
}
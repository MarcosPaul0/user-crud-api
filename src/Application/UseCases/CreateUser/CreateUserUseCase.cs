using UserCrud.Application.Dtos;
using UserCrud.Application.Exceptions;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.CreateUser;

public class CreateUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICreateUserUseCase
{
    public async Task ExecuteAsync(CreateUserDto createUserDto, CancellationToken cancellationToken)
    {
        var userAlreadyExists = await userRepository.FindByEmailAsync(createUserDto.Email, cancellationToken);

        if (userAlreadyExists != null)
        {
            throw new ConflictException(ExceptionMessages.USER_ALREADY_EXISTS);
        }
        
        var newUser = new User(createUserDto.Name, createUserDto.Email, createUserDto.Password);
        
        await userRepository.CreateAsync(newUser, cancellationToken);
        await unitOfWork.SaveChangesAsync();
    }
}
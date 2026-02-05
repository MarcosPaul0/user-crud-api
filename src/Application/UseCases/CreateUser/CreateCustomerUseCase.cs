using UserCrud.Application.Dtos;
using UserCrud.Application.Exceptions;
using UserCrud.Application.Interfaces;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Enums;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.CreateUser;

public class CreateCustomerUseCase(
    IPasswordHasherService passwordHasherService, 
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork) : ICreateCustomerUseCase
{
    public async Task ExecuteAsync(CreateUserDto createUserDto, CancellationToken cancellationToken)
    {
        var userAlreadyExists = await userRepository.FindByEmailAsync(createUserDto.Email, cancellationToken);

        if (userAlreadyExists != null)
        {
            throw new ConflictException(ExceptionMessages.USER_ALREADY_EXISTS);
        }
        
        var passwordHash = passwordHasherService.Hash(createUserDto.Password);
        
        var newUser = new User(createUserDto.Name, createUserDto.Email, passwordHash, UserRole.Customer, DateTime.UtcNow);
        
        await userRepository.CreateAsync(newUser, cancellationToken);
        await unitOfWork.SaveChangesAsync();
    }
}
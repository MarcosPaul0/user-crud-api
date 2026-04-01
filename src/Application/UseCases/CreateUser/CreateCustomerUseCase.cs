using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces;
using AutoriaStore.Application.Interfaces;

namespace AutoriaStore.Application.UseCases.CreateUser;

public sealed class CreateCustomerUseCase(
    IPasswordHasherService passwordHasherService,
    IUnitOfWork unitOfWork) : ICreateCustomerUseCase
{
    public async Task ExecuteAsync(CreateUserDto createUserDto, CancellationToken cancellationToken)
    {
        var userAlreadyExists = await unitOfWork.User.FindByEmailAsync(createUserDto.Email, cancellationToken);

        if (userAlreadyExists != null)
        {
            throw new ConflictException(ExceptionMessages.USER_ALREADY_EXISTS);
        }
        
        var passwordHash = passwordHasherService.Hash(createUserDto.Password);
        
        var newUser = new User(createUserDto.Name, createUserDto.Email, passwordHash, UserRole.Customer, DateTime.UtcNow);
        
        await unitOfWork.User.CreateAsync(newUser, cancellationToken);
        await unitOfWork.SaveChangesAsync();
    }
}
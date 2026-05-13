// <copyright file="CreateCustomerUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.Application.UseCases.CreateUser;

public sealed class CreateCustomerUseCase(
    IPasswordHasherService passwordHasherService,
    IEmailService emailService,
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

        await emailService.SendAsync(
            new SendEmailDto
            {
                To = newUser.Email,
                Subject = "Bem-vindo(a) a Autoria Store",
                HtmlBody =
                $"""
                 <h1>Cadastro realizado com sucesso</h1>
                 <p>Olá, {newUser.Name}.</p>
                 <p>Sua conta na Autoria Store foi criada com sucesso.</p>
                 """,
            }, cancellationToken);
    }
}

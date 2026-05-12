// <copyright file="ICreateCustomerUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.CreateUser;

public interface ICreateCustomerUseCase
{
    Task ExecuteAsync(CreateUserDto createUserDto, CancellationToken cancellationToken);
}
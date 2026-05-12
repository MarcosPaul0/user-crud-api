// <copyright file="CreateCustomerUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.CreateUser;
using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.UnitTests.UseCases.User;

public class CreateCustomerUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly Mock<IPasswordHasherService> passwordHasherMock;
    private readonly Mock<IEmailService> emailServiceMock;
    private readonly CreateCustomerUseCase sut;

    public CreateCustomerUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.userRepositoryMock = new Mock<IUserRepository>();
        this.passwordHasherMock = new Mock<IPasswordHasherService>();
        this.emailServiceMock = new Mock<IEmailService>();

        this.unitOfWorkMock.Setup(u => u.User).Returns(this.userRepositoryMock.Object);

        this.sut = new CreateCustomerUseCase(this.passwordHasherMock.Object, this.emailServiceMock.Object, this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserAlreadyExists_ThrowsConflictException()
    {
        var dto = new CreateUserDto
        {
            Name = "John Doe Test",
            Email = "john@example.com",
            Password = "password1234",
        };

        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", Domain.Enums.UserRole.Customer, DateTime.UtcNow);

        this.userRepositoryMock
            .Setup(r => r.FindByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var act = () => this.sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.USERALREADYEXISTS, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserDoesNotExist_CreatesUserAndSavesChanges()
    {
        var dto = new CreateUserDto
        {
            Name = "John Doe Test",
            Email = "john@example.com",
            Password = "password1234",
        };

        this.userRepositoryMock
            .Setup(r => r.FindByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.User?)null);

        this.passwordHasherMock
            .Setup(p => p.Hash(dto.Password))
            .Returns("hashed_password");

        await this.sut.ExecuteAsync(dto, CancellationToken.None);

        this.userRepositoryMock.Verify(
            r => r.CreateAsync(
            It.Is<AutoriaStore.Domain.Entities.User>(u =>
                u.Name == dto.Name &&
                u.Email == dto.Email &&
                u.Password == "hashed_password" &&
                u.Role == Domain.Enums.UserRole.Customer),
            It.IsAny<CancellationToken>()), Times.Once);

        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        this.emailServiceMock.Verify(
            service => service.SendAsync(
            It.Is<SendEmailDto>(email =>
                email.To == dto.Email &&
                email.Subject == "Bem-vindo(a) a Autoria Store" &&
                email.HtmlBody.Contains(dto.Name)),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}

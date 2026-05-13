// <copyright file="LoginUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.Login;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.UnitTests.UseCases.User;

public class LoginUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly Mock<IPasswordHasherService> passwordHasherMock;
    private readonly Mock<IJwtTokenService> jwtTokenServiceMock;
    private readonly LoginUseCase sut;

    public LoginUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.userRepositoryMock = new Mock<IUserRepository>();
        this.passwordHasherMock = new Mock<IPasswordHasherService>();
        this.jwtTokenServiceMock = new Mock<IJwtTokenService>();

        this.unitOfWorkMock.Setup(u => u.User).Returns(this.userRepositoryMock.Object);

        this.sut = new LoginUseCase(this.passwordHasherMock.Object, this.unitOfWorkMock.Object, this.jwtTokenServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserNotFound_ThrowsUnauthorizeException()
    {
        var dto = new LoginDto { Email = "notfound@example.com", Password = "password1234" };

        this.userRepositoryMock
            .Setup(r => r.FindByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.User?)null);

        var act = () => this.sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizeException>(act);
        Assert.Equal(ExceptionMessages.LOGIN_FAILED, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPasswordIsInvalid_ThrowsUnauthorizeException()
    {
        var dto = new LoginDto { Email = "john@example.com", Password = "wrongpassword" };

        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hashed_correct_password", UserRole.Customer, DateTime.UtcNow);

        this.userRepositoryMock
            .Setup(r => r.FindByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        this.passwordHasherMock
            .Setup(p => p.Verify(dto.Password, existingUser.Password))
            .Returns(false);

        var act = () => this.sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizeException>(act);
        Assert.Equal(ExceptionMessages.LOGIN_FAILED, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCredentialsAreValid_ReturnsToken()
    {
        var dto = new LoginDto { Email = "john@example.com", Password = "correctpassword" };

        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hashed_password", UserRole.Customer, DateTime.UtcNow)
        {
            Id = Guid.NewGuid(),
        };

        this.userRepositoryMock
            .Setup(r => r.FindByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        this.passwordHasherMock
            .Setup(p => p.Verify(dto.Password, existingUser.Password))
            .Returns(true);

        this.jwtTokenServiceMock
            .Setup(j => j.GenerateToken(existingUser.Id, existingUser.Role))
            .Returns("jwt_token");

        var result = await this.sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Equal("jwt_token", result);
    }
}

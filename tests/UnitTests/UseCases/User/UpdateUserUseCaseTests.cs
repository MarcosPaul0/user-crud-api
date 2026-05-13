// <copyright file="UpdateUserUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.UpdateUser;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.User;

public class UpdateUserUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly UpdateUserUseCase sut;

    public UpdateUserUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.userRepositoryMock = new Mock<IUserRepository>();

        this.unitOfWorkMock.Setup(u => u.User).Returns(this.userRepositoryMock.Object);

        this.sut = new UpdateUserUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        var userId = Guid.NewGuid();
        var dto = new UpdateUserDto { Name = "New Name Test" };

        this.userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.User?)null);

        var act = () => this.sut.ExecuteAsync(userId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.USER_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNothingChanged_DoesNotUpdateOrSave()
    {
        var userId = Guid.NewGuid();
        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId,
        };

        var dto = new UpdateUserDto
        {
            Name = existingUser.Name,
            Email = existingUser.Email,
            Password = existingUser.Password,
        };

        this.userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await this.sut.ExecuteAsync(userId, dto, CancellationToken.None);

        this.userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AutoriaStore.Domain.Entities.User>(), It.IsAny<CancellationToken>()), Times.Never);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNameChanged_UpdatesUserAndSavesChanges()
    {
        var userId = Guid.NewGuid();
        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId,
        };

        var dto = new UpdateUserDto { Name = "Jane Doe Updated" };

        this.userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await this.sut.ExecuteAsync(userId, dto, CancellationToken.None);

        Assert.Equal("Jane Doe Updated", existingUser.Name);
        this.userRepositoryMock.Verify(r => r.UpdateAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmailChanged_UpdatesUserAndSavesChanges()
    {
        var userId = Guid.NewGuid();
        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId,
        };

        var dto = new UpdateUserDto { Email = "newemail@example.com" };

        this.userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await this.sut.ExecuteAsync(userId, dto, CancellationToken.None);

        Assert.Equal("newemail@example.com", existingUser.Email);
        this.userRepositoryMock.Verify(r => r.UpdateAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPasswordChanged_UpdatesUserAndSavesChanges()
    {
        var userId = Guid.NewGuid();
        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "oldhash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId,
        };

        var dto = new UpdateUserDto { Password = "newhashpassword" };

        this.userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await this.sut.ExecuteAsync(userId, dto, CancellationToken.None);

        Assert.Equal("newhashpassword", existingUser.Password);
        this.userRepositoryMock.Verify(r => r.UpdateAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

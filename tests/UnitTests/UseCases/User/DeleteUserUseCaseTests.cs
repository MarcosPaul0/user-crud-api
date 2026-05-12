// <copyright file="DeleteUserUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.DeleteUser;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.User;

public class DeleteUserUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly DeleteUserUseCase sut;

    public DeleteUserUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.userRepositoryMock = new Mock<IUserRepository>();

        this.unitOfWorkMock.Setup(u => u.User).Returns(this.userRepositoryMock.Object);

        this.sut = new DeleteUserUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        var userId = Guid.NewGuid();

        this.userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.User?)null);

        var act = () => this.sut.ExecuteAsync(userId, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.USERNOTFOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserFound_DeletesUserAndSavesChanges()
    {
        var userId = Guid.NewGuid();
        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId,
        };

        this.userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        this.userRepositoryMock
            .Setup(r => r.DeleteAsync(existingUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await this.sut.ExecuteAsync(userId, CancellationToken.None);

        this.userRepositoryMock.Verify(r => r.DeleteAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
        this.unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

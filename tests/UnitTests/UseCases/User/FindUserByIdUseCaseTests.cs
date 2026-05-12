// <copyright file="FindUserByIdUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.FindUserById;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.User;

public class FindUserByIdUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly FindUserByIdUseCase sut;

    public FindUserByIdUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.userRepositoryMock = new Mock<IUserRepository>();

        this.unitOfWorkMock.Setup(u => u.User).Returns(this.userRepositoryMock.Object);

        this.sut = new FindUserByIdUseCase(this.unitOfWorkMock.Object);
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
    public async Task ExecuteAsync_WhenUserFound_ReturnsUser()
    {
        var userId = Guid.NewGuid();
        var expectedUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId,
        };

        this.userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        var result = await this.sut.ExecuteAsync(userId, CancellationToken.None);

        Assert.Equal(expectedUser, result);
    }
}

// <copyright file="ListUserUseCaseTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.ListUsers;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.User;

public class ListUserUseCaseTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly ListUserUseCase sut;

    public ListUserUseCaseTests()
    {
        this.unitOfWorkMock = new Mock<IUnitOfWork>();
        this.userRepositoryMock = new Mock<IUserRepository>();

        this.unitOfWorkMock.Setup(u => u.User).Returns(this.userRepositoryMock.Object);

        this.sut = new ListUserUseCase(this.unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsUsersAndCount()
    {
        var dto = new ListUsersDto { Page = 1, ItemsPerPage = 10, Name = "John", Role = UserRole.Customer };

        var users = new List<AutoriaStore.Domain.Entities.User>
        {
            new ("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow),
            new ("John Smith Test", "jsmith@example.com", "hash", UserRole.Customer, DateTime.UtcNow),
        };
        var totalCount = 2;

        this.userRepositoryMock
            .Setup(r => r.FindAllAsync(It.IsAny<AutoriaStore.Domain.Entities.User>(), dto.Page, dto.ItemsPerPage, It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        this.userRepositoryMock
            .Setup(r => r.CountAsync(It.IsAny<AutoriaStore.Domain.Entities.User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalCount);

        var (resultUsers, resultCount) = await this.sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Equal(users, resultUsers);
        Assert.Equal(totalCount, resultCount);
    }
}

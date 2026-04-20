using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.UseCases.ListUsers;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces;
using Moq;

namespace AutoriaStore.UnitTests.UseCases.User;

public class ListUserUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly ListUserUseCase _sut;

    public ListUserUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _unitOfWorkMock.Setup(u => u.User).Returns(_userRepositoryMock.Object);

        _sut = new ListUserUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsUsersAndCount()
    {
        var dto = new ListUsersDto { Page = 1, ItemsPerPage = 10, Name = "John", Role = UserRole.Customer };

        var users = new List<AutoriaStore.Domain.Entities.User>
        {
            new("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow),
            new("John Smith Test", "jsmith@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        };
        var totalCount = 2;

        _userRepositoryMock
            .Setup(r => r.FindAllAsync(It.IsAny<AutoriaStore.Domain.Entities.User>(), dto.Page, dto.ItemsPerPage, It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        _userRepositoryMock
            .Setup(r => r.CountAsync(It.IsAny<AutoriaStore.Domain.Entities.User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalCount);

        var (resultUsers, resultCount) = await _sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Equal(users, resultUsers);
        Assert.Equal(totalCount, resultCount);
    }
}

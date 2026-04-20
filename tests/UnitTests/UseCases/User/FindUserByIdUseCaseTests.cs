using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.FindUserById;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces;
using Moq;

namespace AutoriaStore.UnitTests.UseCases.User;

public class FindUserByIdUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly FindUserByIdUseCase _sut;

    public FindUserByIdUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _unitOfWorkMock.Setup(u => u.User).Returns(_userRepositoryMock.Object);

        _sut = new FindUserByIdUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        var userId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.User?)null);

        var act = () => _sut.ExecuteAsync(userId, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.USER_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserFound_ReturnsUser()
    {
        var userId = Guid.NewGuid();
        var expectedUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId
        };

        _userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        var result = await _sut.ExecuteAsync(userId, CancellationToken.None);

        Assert.Equal(expectedUser, result);
    }
}

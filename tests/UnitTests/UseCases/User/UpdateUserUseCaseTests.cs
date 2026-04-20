using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.UpdateUser;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces;
using Moq;

namespace AutoriaStore.UnitTests.UseCases.User;

public class UpdateUserUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UpdateUserUseCase _sut;

    public UpdateUserUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _unitOfWorkMock.Setup(u => u.User).Returns(_userRepositoryMock.Object);

        _sut = new UpdateUserUseCase(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserNotFound_ThrowsNotFoundException()
    {
        var userId = Guid.NewGuid();
        var dto = new UpdateUserDto { Name = "New Name Test" };

        _userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.User?)null);

        var act = () => _sut.ExecuteAsync(userId, dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<NotFoundException>(act);
        Assert.Equal(ExceptionMessages.USER_NOT_FOUND, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNothingChanged_DoesNotUpdateOrSave()
    {
        var userId = Guid.NewGuid();
        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId
        };

        var dto = new UpdateUserDto
        {
            Name = existingUser.Name,
            Email = existingUser.Email,
            Password = existingUser.Password
        };

        _userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await _sut.ExecuteAsync(userId, dto, CancellationToken.None);

        _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<AutoriaStore.Domain.Entities.User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNameChanged_UpdatesUserAndSavesChanges()
    {
        var userId = Guid.NewGuid();
        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId
        };

        var dto = new UpdateUserDto { Name = "Jane Doe Updated" };

        _userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await _sut.ExecuteAsync(userId, dto, CancellationToken.None);

        Assert.Equal("Jane Doe Updated", existingUser.Name);
        _userRepositoryMock.Verify(r => r.UpdateAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmailChanged_UpdatesUserAndSavesChanges()
    {
        var userId = Guid.NewGuid();
        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId
        };

        var dto = new UpdateUserDto { Email = "newemail@example.com" };

        _userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await _sut.ExecuteAsync(userId, dto, CancellationToken.None);

        Assert.Equal("newemail@example.com", existingUser.Email);
        _userRepositoryMock.Verify(r => r.UpdateAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPasswordChanged_UpdatesUserAndSavesChanges()
    {
        var userId = Guid.NewGuid();
        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "oldhash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId
        };

        var dto = new UpdateUserDto { Password = "newhashpassword" };

        _userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await _sut.ExecuteAsync(userId, dto, CancellationToken.None);

        Assert.Equal("newhashpassword", existingUser.Password);
        _userRepositoryMock.Verify(r => r.UpdateAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

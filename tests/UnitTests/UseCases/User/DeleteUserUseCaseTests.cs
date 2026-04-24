using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.DeleteUser;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.UnitTests.UseCases.User;

public class DeleteUserUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly DeleteUserUseCase _sut;

    public DeleteUserUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _unitOfWorkMock.Setup(u => u.User).Returns(_userRepositoryMock.Object);

        _sut = new DeleteUserUseCase(_unitOfWorkMock.Object);
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
    public async Task ExecuteAsync_WhenUserFound_DeletesUserAndSavesChanges()
    {
        var userId = Guid.NewGuid();
        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", UserRole.Customer, DateTime.UtcNow)
        {
            Id = userId
        };

        _userRepositoryMock
            .Setup(r => r.FindByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        _userRepositoryMock
            .Setup(r => r.DeleteAsync(existingUser, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        await _sut.ExecuteAsync(userId, CancellationToken.None);

        _userRepositoryMock.Verify(r => r.DeleteAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

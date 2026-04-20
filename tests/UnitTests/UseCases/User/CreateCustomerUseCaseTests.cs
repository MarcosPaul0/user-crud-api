using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.Interfaces;
using AutoriaStore.Application.UseCases.CreateUser;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using Moq;

namespace AutoriaStore.UnitTests.UseCases.User;

public class CreateCustomerUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasherService> _passwordHasherMock;
    private readonly CreateCustomerUseCase _sut;

    public CreateCustomerUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasherService>();

        _unitOfWorkMock.Setup(u => u.User).Returns(_userRepositoryMock.Object);

        _sut = new CreateCustomerUseCase(_passwordHasherMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserAlreadyExists_ThrowsConflictException()
    {
        var dto = new CreateUserDto
        {
            Name = "John Doe Test",
            Email = "john@example.com",
            Password = "password1234"
        };

        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hash", Domain.Enums.UserRole.Customer, DateTime.UtcNow);

        _userRepositoryMock
            .Setup(r => r.FindByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var act = () => _sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ConflictException>(act);
        Assert.Equal(ExceptionMessages.USER_ALREADY_EXISTS, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserDoesNotExist_CreatesUserAndSavesChanges()
    {
        var dto = new CreateUserDto
        {
            Name = "John Doe Test",
            Email = "john@example.com",
            Password = "password1234"
        };

        _userRepositoryMock
            .Setup(r => r.FindByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.User?)null);

        _passwordHasherMock
            .Setup(p => p.Hash(dto.Password))
            .Returns("hashed_password");

        await _sut.ExecuteAsync(dto, CancellationToken.None);

        _userRepositoryMock.Verify(r => r.CreateAsync(
            It.Is<AutoriaStore.Domain.Entities.User>(u =>
                u.Name == dto.Name &&
                u.Email == dto.Email &&
                u.Password == "hashed_password" &&
                u.Role == Domain.Enums.UserRole.Customer),
            It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}

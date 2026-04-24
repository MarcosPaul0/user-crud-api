using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Application.UseCases.Login;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;

namespace AutoriaStore.UnitTests.UseCases.User;

public class LoginUseCaseTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasherService> _passwordHasherMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly LoginUseCase _sut;

    public LoginUseCaseTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasherService>();
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();

        _unitOfWorkMock.Setup(u => u.User).Returns(_userRepositoryMock.Object);

        _sut = new LoginUseCase(_passwordHasherMock.Object, _unitOfWorkMock.Object, _jwtTokenServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserNotFound_ThrowsUnauthorizeException()
    {
        var dto = new LoginDto { Email = "notfound@example.com", Password = "password1234" };

        _userRepositoryMock
            .Setup(r => r.FindByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((AutoriaStore.Domain.Entities.User?)null);

        var act = () => _sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizeException>(act);
        Assert.Equal(ExceptionMessages.LOGIN_FAILED, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPasswordIsInvalid_ThrowsUnauthorizeException()
    {
        var dto = new LoginDto { Email = "john@example.com", Password = "wrongpassword" };

        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hashed_correct_password", UserRole.Customer, DateTime.UtcNow);

        _userRepositoryMock
            .Setup(r => r.FindByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        _passwordHasherMock
            .Setup(p => p.Verify(dto.Password, existingUser.Password))
            .Returns(false);

        var act = () => _sut.ExecuteAsync(dto, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<UnauthorizeException>(act);
        Assert.Equal(ExceptionMessages.LOGIN_FAILED, exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_WhenCredentialsAreValid_ReturnsToken()
    {
        var dto = new LoginDto { Email = "john@example.com", Password = "correctpassword" };

        var existingUser = new AutoriaStore.Domain.Entities.User("John Doe Test", "john@example.com", "hashed_password", UserRole.Customer, DateTime.UtcNow)
        {
            Id = Guid.NewGuid()
        };

        _userRepositoryMock
            .Setup(r => r.FindByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        _passwordHasherMock
            .Setup(p => p.Verify(dto.Password, existingUser.Password))
            .Returns(true);

        _jwtTokenServiceMock
            .Setup(j => j.GenerateToken(existingUser.Id, existingUser.Role))
            .Returns("jwt_token");

        var result = await _sut.ExecuteAsync(dto, CancellationToken.None);

        Assert.Equal("jwt_token", result);
    }
}

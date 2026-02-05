using UserCrud.Application.Dtos;
using UserCrud.Application.Exceptions;
using UserCrud.Application.Interfaces;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.Login;

public class LoginUseCase(
    IPasswordHasherService passwordHasherService, 
    IUserRepository userRepository, 
    IJwtTokenService jwtTokenService) : ILoginUseCase
{
    public async Task<string> ExecuteAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByEmailAsync(loginDto.Email, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizeException(ExceptionMessages.LOGIN_FAILED);
        }
        
        var passwordIsValid = passwordHasherService.Verify(loginDto.Password, user.Password);

        if (!passwordIsValid)
        {
            throw new UnauthorizeException(ExceptionMessages.LOGIN_FAILED);
        }

        var token = jwtTokenService.GenerateToken(user.Id, user.Role);

        return token;
    }
}
using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.Login;

public interface ILoginUseCase
{
    Task<string> ExecuteAsync(LoginDto loginDto, CancellationToken cancellationToken);
}
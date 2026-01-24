namespace UserCrud.Application.UseCases.DeleteUser;

public interface IDeleteUserUseCase
{
    Task ExecuteAsync(Guid userId, CancellationToken cancellationToken);
}
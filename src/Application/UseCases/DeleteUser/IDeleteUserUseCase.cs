namespace AutoriaStore.Application.UseCases.DeleteUser;

public interface IDeleteUserUseCase
{
    Task ExecuteAsync(Guid userId, CancellationToken cancellationToken);
}
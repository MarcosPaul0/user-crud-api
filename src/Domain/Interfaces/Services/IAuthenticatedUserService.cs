namespace AutoriaStore.Domain.Interfaces.Services;

public interface IAuthenticatedUserService
{
    Guid? GetUserId();
}

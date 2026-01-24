using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.ListUsers;

public class ListUserUseCase(IUserRepository userRepository) : IListUserUseCase
{
    public async Task<IEnumerable<User>> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await userRepository.FindAllAsync(cancellationToken);
    }
}
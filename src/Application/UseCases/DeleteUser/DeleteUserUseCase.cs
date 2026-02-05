using UserCrud.Application.Exceptions;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.DeleteUser;

public class DeleteUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork) : IDeleteUserUseCase
{
    public async Task ExecuteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(ExceptionMessages.USER_NOT_FOUND);
        }
        
        await userRepository.DeleteAsync(user, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();
    }
}
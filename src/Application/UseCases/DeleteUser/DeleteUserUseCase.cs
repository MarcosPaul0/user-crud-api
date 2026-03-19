using UserCrud.Application.Exceptions;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.DeleteUser;

public sealed class DeleteUserUseCase(IUnitOfWork unitOfWork) : IDeleteUserUseCase
{
    public async Task ExecuteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.User.FindByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(ExceptionMessages.USER_NOT_FOUND);
        }
        
        await unitOfWork.User.DeleteAsync(user, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();
    }
}
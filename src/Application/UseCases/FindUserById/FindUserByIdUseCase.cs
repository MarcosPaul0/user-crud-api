using UserCrud.Application.Exceptions;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.FindUserById;

public sealed class FindUserByIdUseCase(IUnitOfWork unitOfWork) : IFindUserByIdUseCase
{
    public async Task<User> ExecuteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.User.FindByIdAsync(userId, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException(ExceptionMessages.USER_NOT_FOUND);
        }
        
        return user;
    }
}
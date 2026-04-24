using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.FindUserById;

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
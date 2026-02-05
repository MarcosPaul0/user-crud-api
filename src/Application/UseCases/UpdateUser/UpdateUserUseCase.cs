using UserCrud.Application.Dtos;
using UserCrud.Application.Exceptions;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.UpdateUser;

public class UpdateUserUseCase(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUpdateUserUseCase
{
    public async Task ExecuteAsync(Guid userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(userId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(ExceptionMessages.USER_NOT_FOUND);
        }

        var hasBeenUpdated = false;
        
        if (updateUserDto.Email != null && user.Email != updateUserDto.Email)
        {
            user.Email = updateUserDto.Email;
            hasBeenUpdated = true;
        }
        
        if (updateUserDto.Name != null && user.Name != updateUserDto.Name)
        {
            user.Name = updateUserDto.Name;
            hasBeenUpdated = true;
        }
        
        if (updateUserDto.Password != null && user.Password != updateUserDto.Password)
        {
            user.Password = updateUserDto.Password;
            hasBeenUpdated = true;
        }

        if (hasBeenUpdated)
        {
            user.UpdatedAt = DateTime.UtcNow;
            
            await userRepository.UpdateAsync(user);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
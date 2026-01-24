using Microsoft.Extensions.DependencyInjection;
using UserCrud.Application.UseCases.CreateUser;
using UserCrud.Application.UseCases.DeleteUser;
using UserCrud.Application.UseCases.FindUserById;
using UserCrud.Application.UseCases.ListUsers;
using UserCrud.Application.UseCases.UpdateUser;

namespace UserCrud.Application.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        services.AddScoped<IFindUserByIdUseCase, FindUserByIdUseCase>();
        services.AddScoped<IListUserUseCase, ListUserUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();

        return services;
    }
}
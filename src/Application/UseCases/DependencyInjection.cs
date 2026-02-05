using Microsoft.Extensions.DependencyInjection;
using UserCrud.Application.UseCases.CreateProduct;
using UserCrud.Application.UseCases.CreateProductCategory;
using UserCrud.Application.UseCases.CreateUser;
using UserCrud.Application.UseCases.DeleteUser;
using UserCrud.Application.UseCases.FindProductById;
using UserCrud.Application.UseCases.FindUserById;
using UserCrud.Application.UseCases.ListProductCategory;
using UserCrud.Application.UseCases.ListProducts;
using UserCrud.Application.UseCases.ListUsers;
using UserCrud.Application.UseCases.Login;
using UserCrud.Application.UseCases.UpdateProduct;
using UserCrud.Application.UseCases.UpdateProductCategory;
using UserCrud.Application.UseCases.UpdateUser;

namespace UserCrud.Application.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();
        services.AddScoped<IFindUserByIdUseCase, FindUserByIdUseCase>();
        services.AddScoped<IListUserUseCase, ListUserUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        
        services.AddScoped<ICreateProductCategoryUseCase, CreateProductCategoryUseCase>();
        services.AddScoped<IUpdateProductCategoryUseCase, UpdateProductCategoryUseCase>();
        services.AddScoped<IListProductCategoryUseCase, ListProductCategoryUseCase>();
        
        services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
        services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
        services.AddScoped<IListProductsUseCase, ListProductsUseCase>();
        services.AddScoped<IFindProductByIdUseCase, IFindProductByIdUseCase>();

        return services;
    }
}
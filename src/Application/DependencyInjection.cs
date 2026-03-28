using AutoriaStore.Application.UseCases.CreateProductCategory;
using AutoriaStore.Application.UseCases.ListProductCategory;
using AutoriaStore.Application.UseCases.ListProductCategoryForAdmin;
using AutoriaStore.Application.UseCases.UpdateProductCategory;
using Microsoft.Extensions.DependencyInjection;
using AutoriaStore.Application.UseCases.CreateProduct;
using AutoriaStore.Application.UseCases.CreateUser;
using AutoriaStore.Application.UseCases.DeleteProduct;
using AutoriaStore.Application.UseCases.DeleteProductCategory;
using AutoriaStore.Application.UseCases.DeleteUser;
using AutoriaStore.Application.UseCases.FindProductById;
using AutoriaStore.Application.UseCases.FindProductCategoryById;
using AutoriaStore.Application.UseCases.FindUserById;
using AutoriaStore.Application.UseCases.ListProducts;
using AutoriaStore.Application.UseCases.ListProductsForAdmin;
using AutoriaStore.Application.UseCases.ListUsers;
using AutoriaStore.Application.UseCases.Login;
using AutoriaStore.Application.UseCases.SetProductImages;
using AutoriaStore.Application.UseCases.UpdateProduct;
using AutoriaStore.Application.UseCases.UpdateUser;

namespace AutoriaStore.Application;

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
        
        services.AddScoped<IFindProductCategoryByIdUseCase, FindProductCategoryByIdUseCase>();
        services.AddScoped<ICreateProductCategoryUseCase, CreateProductCategoryUseCase>();
        services.AddScoped<IUpdateProductCategoryUseCase, UpdateProductCategoryUseCase>();
        services.AddScoped<IListProductCategoryUseCase, ListProductCategoryUseCase>();
        services.AddScoped<IListProductCategoryForAdminUseCase, ListProductCategoryForAdminUseCase>();
        services.AddScoped<IDeleteProductCategoryUseCase, DeleteProductCategoryUseCase>();
        
        services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
        services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
        services.AddScoped<IListProductsUseCase, ListProductsUseCase>();
        services.AddScoped<IListProductsForAdminUseCase, ListProductsForAdminUseCase>();
        services.AddScoped<IFindProductByIdUseCase, FindProductByIdUseCase>();
        services.AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();

        services.AddScoped<ISetProductImagesUseCase, SetProductImagesUseCase>();

        return services;
    }
}
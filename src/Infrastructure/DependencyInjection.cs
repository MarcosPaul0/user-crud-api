using Amazon.Runtime;
using Amazon.S3;
using AutoriaStore.Infrastructure.Context;
using AutoriaStore.Infrastructure.Repositories;
using AutoriaStore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoriaStore.Domain.Interfaces.Clients;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;
using AutoriaStore.Infrastructure.Clients;

namespace AutoriaStore.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IEnvironmentVariablesService, EnvironmentVariablesService>();

        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        
        Console.WriteLine(connectionString);
        
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("The database connection string is not set.");
        }
        
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
        services.AddHttpContextAccessor();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPhoneRepository, PhoneRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        services.AddScoped<IIdempotencyKeyRepository, IdempotencyKeyRepository>();
        
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IIdempotencyService, IdempotencyService>();
        services.AddScoped<IPostageHttpClient, CorreiosHttpClient>();
        services.AddSingleton<IAmazonS3>(serviceProvider =>
        {
            var environmentVariablesService = serviceProvider.GetRequiredService<IEnvironmentVariablesService>();
            
            var credentials = new BasicAWSCredentials(
                environmentVariablesService.ObjectStorageAccessKey, 
                environmentVariablesService.ObjectStorageSecretKey);
            
            var config = new AmazonS3Config
            {
                ServiceURL = environmentVariablesService.ObjectStorageEndpoint,
                ForcePathStyle = true
            };

            return new AmazonS3Client(credentials, config);
        });
        services.AddScoped<IObjectStorageService, CloudflareR2Service>();
    }
}

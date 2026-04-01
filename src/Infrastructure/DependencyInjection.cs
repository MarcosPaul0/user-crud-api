using Amazon.Runtime;
using Amazon.S3;
using AutoriaStore.Domain.Interfaces;
using AutoriaStore.Infrastructure.Context;
using AutoriaStore.Infrastructure.Repositories;
using AutoriaStore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AutoriaStore.Application.Interfaces;
using AutoriaStore.Infrastructure.Repositories;

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

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPhoneRepository, PhoneRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();
        
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
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
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserCrud.Application.Interfaces;
using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;
using UserCrud.Infrastructure.Repositories;
using UserCrud.Infrastructure.Services;

namespace UserCrud.Infrastructure;

public static class DependencyInjection
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPhoneRepository, PhoneRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductImageRepository, ProductImageRepository>();

        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IEnvironmentVariablesService, EnvironmentVariablesService>();
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
    }
}
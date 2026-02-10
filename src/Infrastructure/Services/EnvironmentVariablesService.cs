using UserCrud.Application.Interfaces;

namespace UserCrud.Infrastructure.Services;

public class EnvironmentVariablesService : IEnvironmentVariablesService
{
    public string JwtPrivateKey { get; private set; } = GetRequiredAndConvert<string>("JWT_PRIVATE_KEY");

    public string JwtPublicKey { get; private set; } = GetRequiredAndConvert<string>("JWT_PUBLIC_KEY");
    public string JwtIssuer { get; private set; } = GetRequiredAndConvert<string>("JWT_ISSUER");
    public string JwtAudience { get; private set; } = GetRequiredAndConvert<string>("JWT_AUDIENCE");
    public int JwtExpirationTimeInMinutes { get; private set; } = GetRequiredAndConvert<int>("JWT_EXPIRATION_TIME_IN_MINUTES");
    public string ObjectStorageBucket { get; private set; } = GetRequiredAndConvert<string>("OBJECT_STORAGE_BUCKET");
    public string ObjectStorageEndpoint { get; private set; } = GetRequiredAndConvert<string>("OBJECT_STORAGE_ENDPOINT");
    public string ObjectStoragePublicUrl { get; private set; } = GetRequiredAndConvert<string>("OBJECT_STORAGE_PUBLIC_URL");
    public string ObjectStorageAccessKey { get; private set; } = GetRequiredAndConvert<string>("OBJECT_STORAGE_ACCESS_KEY");
    public string ObjectStorageSecretKey { get; private set; } = GetRequiredAndConvert<string>("OBJECT_STORAGE_SECRET_KEY");

    private static T GetRequiredAndConvert<T>(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);
        
        Console.WriteLine($"Environment variable '{variableName}' has value '{value}'.");
        
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"⁉️⁉️ The environment variable '{variableName}' is required and has not been set.");
        }
        
        try
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"⁉️⁉️ Error converting variable '{variableName}' with value '{value}' to type {typeof(T).Name}.", ex);
        }
    }
}